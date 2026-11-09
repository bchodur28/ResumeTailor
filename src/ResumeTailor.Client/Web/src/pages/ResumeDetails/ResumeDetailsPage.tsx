import { useEffect, useState, useRef, useMemo } from "react";
import { useFieldArray, useForm, useWatch } from "react-hook-form";
import type { ResumeDetailsForm } from "../../models/forms/ResumeDetailsForm";
import { useParams } from "react-router-dom";
import { useResumeDetails } from "../../hooks/useResumeDetails";
import Card from "../../components/ui/Card";
import RatingBar from "../../components/ui/feedback/RatingBar";
import type { ResumeResponse } from "../../api/contracts/resumes/ResumeResponse";
import type { ResumeEducationRequest } from "../../api/contracts/resumes/ResumeEducationRequest";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useLibraryEducation } from "../../hooks/useLibraryEducation";
import {
  Book,
  Folder2Open,
  TypeH2,
  TypeH1,
  Paragraph,
  PatchCheck,
} from "react-bootstrap-icons";
import { useCompanyBullets } from "../../hooks/useCompanyBullet";
import { getResumePdf } from "../../api/resumeApi";
import { useUpdateResume } from "../../hooks/useUpdateResume";
import Message from "../../components/ui/feedback/Message";
import ListItem from "../../components/ui/lists/ListItem";
import ScaledResumePreview from "./components/ScaledResumePreview";
import ButtonContainer from "../../components/ui/buttons/ButtonContainer";
import DropdownButton from "../../components/ui/buttons/DropdownButton";
import SearchDropdown from "./components/SearchDropdown";
import type { UpdateResumeRequest } from "../../api/contracts/resumes/UpdateResumeRequest";
import type { ResumeExperienceRequest } from "../../api/contracts/resumes/ResumeExperienceRequest";
import { useLibraryExperiences } from "../../hooks/useLibraryExperiences";
import type { ResolvedEducation } from "../../models/education/ResolvedEducation";
import type { LibraryEducationResponse } from "../../api/contracts/education/LibraryEducationResponse";
import type { ResolvedResume } from "../../models/resumes/ResolvedResume";
import type { ResolvedExperience } from "../../models/Experience/ResolvedExperience";
import type { LibraryExperienceResponse } from "../../api/contracts/library/LibraryExperienceResponse";
import Resume from "../../components/resume/Resume";
import { ResumeEditProvider } from "../../contexts/ResumeEditContext";
import type { SkillResponse } from "../../api/contracts/accounts/SkillResponse";
import Dropdown from "./components/Dropdown";

const mapResumeToRequest = (resume: ResumeResponse): UpdateResumeRequest => ({
  name: resume.resumeName ?? "",

  education: resume.education.map((education, index) => ({
    id: education.id,
    libraryEducationId: education.libraryEducationId,
    sortOrder: index + 1,
  })),

  experiences: resume.experiences.map((experience, index) => ({
    id: experience.id,
    libraryExperienceId: experience.libraryExperienceId,
    librarySummaryId: experience.primarySummary?.librarySummaryId ?? null,
    sortOrder: index + 1,
    bullets: experience.bullets.map((bullet, index) => ({
      id: bullet.id,
      libraryBulletId: bullet.libraryBulletId,
      sortOrder: bullet.sortOrder ?? index + 1,
    })),
  })),

  appearance: resume.appearance,
});

const resolveLibraryExperience = (
  experience: LibraryExperienceResponse,
): ResolvedExperience => {
  return {
    ...experience,
    resumeExperienceId: null,
    libraryExperienceId: experience.id,
    sortOrder: 0,
    useInResume: false,

    summaries: (experience.summaries ?? []).map((summary) => ({
      ...summary,
      resumeSummaryId: null,
      librarySummaryId: summary.id,
      wasManuallyChanged: false,
      aiReasoning: null,
      sortOrder: 0,
      useInResume: false,
    })),

    bullets: (experience.bullets ?? []).map((bullet) => ({
      ...bullet,
      resumeBulletId: null,
      libraryBulletId: bullet.id,
      aiAlternativeValue: null,
      aiReasoning: null,
      sortOrder: 0,
      isSourceDeleted: false,
      useInResume: false,
    })),
  };
};

const resolveLibraryEducation = (
  education: LibraryEducationResponse,
): ResolvedEducation => {
  return {
    resumeEducationId: null,
    libraryEducationId: education.id,
    type: education.type,
    institutionName: education.institutionName,
    qualification: education.qualification,
    fieldOfStudy: education.fieldOfStudy,
    gpa: education.gpa,
    location: education.location,
    started: education.started,
    ended: education.ended,
    sortOrder: 0,
    useInResume: false,
  };
};

const ResumeDetailsPage = () => {
  const { id } = useParams();
  const resumeId = Number(id);

  const { data: resumeDetails, isLoading, error } = useResumeDetails(resumeId);
  const { data: availableEducation = [] } = useLibraryEducation();
  const { data: availableLibraryExperiences = [] } = useLibraryExperiences();

  console.log("RESUME DETAILS", availableLibraryExperiences);

  // Latest saved resume state
  const [savedResume, setSavedResume] = useState<ResumeResponse | null>(null);
  const baseResume = savedResume ?? resumeDetails?.resume;

  const educationMapping = useMemo(() => {
    return new Map<number, ResolvedEducation>(
      availableEducation.map((education) => [
        education.id,
        resolveLibraryEducation(education),
      ]),
    );
  }, [availableEducation]);

  const experienceMapping = useMemo(() => {
    const map = new Map<number, ResolvedExperience>();

    // 1. Start with all library experiences
    availableLibraryExperiences.forEach((lib) => {
      map.set(lib.id, resolveLibraryExperience(lib));
    });

    // 2. Merge saved resume selections and AI metadata
    baseResume?.experiences.forEach((resume) => {
      const experience = map.get(resume.libraryExperienceId);

      if (!experience) return;

      experience.resumeExperienceId = resume.id;
      experience.sortOrder = resume.sortOrder;
      experience.useInResume = true;

      // Merge selected summary information
      if (resume.primarySummary) {
        experience.summaries = experience.summaries.map((summary) =>
          summary.librarySummaryId === resume.primarySummary?.librarySummaryId
            ? {
                ...summary,
                resumeSummaryId: resume.primarySummary.id,
                wasManuallyChanged: resume.primarySummary.wasManuallyChanged,
                aiReasoning: resume.primarySummary.aiReasoning,
                sortOrder: resume.primarySummary.sortOrder,
                value: resume.primarySummary.value,
                useInResume: true,
              }
            : summary,
        );
      }

      // Merge selected bullet information
      const resumeBullets = new Map(
        resume.bullets.map((bullet) => [bullet.libraryBulletId, bullet]),
      );

      experience.bullets = experience.bullets.map((bullet) => {
        const selected = resumeBullets.get(bullet.libraryBulletId);

        if (!selected) return bullet;

        return {
          ...bullet,
          resumeBulletId: selected.id,
          value: selected.value,
          aiAlternativeValue: selected.aiAlternativeValue,
          aiReasoning: selected.aiReasoning,
          sortOrder: selected.sortOrder,
          isSourceDeleted: selected.isSourceDeleted,
          useInResume: true,
        };
      });
    });

    return map;
  }, [availableLibraryExperiences, baseResume?.experiences]);

  const updateResumeMutation = useUpdateResume(resumeId);

  const lastSavedRef = useRef<UpdateResumeRequest | null>(null);
  const [revertSnapshot, setRevertSnapshot] =
    useState<UpdateResumeRequest | null>(null);

  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const [deletedBulletsWarning, setDeletedBulletsWarning] = useState(false);
  const deletedBulletsWarningInitialized = useRef(false);

  const { handleSubmit, reset, control, setValue, getValues } =
    useForm<UpdateResumeRequest>({
      defaultValues: {
        name: "",
        education: [] as ResumeEducationRequest[],
        experiences: [] as ResumeExperienceRequest[],
        appearance: {
          titleFontSize: 11,
          sectionHeaderFontSize: 10,
          mainBodyFontSize: 11,
          fontFamily: "calibri, sans-serif",
          fontColor: "#000000",
          topHeaderAlignment: "left",
        },
      },
    });

  useEffect(() => {
    if (!resumeDetails) {
      return;
    }

    setSavedResume(resumeDetails.resume);

    const updateResumeRequest: UpdateResumeRequest = mapResumeToRequest(
      resumeDetails.resume,
    );

    console.log("Resume API experiences:", resumeDetails.resume.experiences);
    console.log("Mapped request experiences:", updateResumeRequest.experiences);

    reset(updateResumeRequest);

    lastSavedRef.current = structuredClone(updateResumeRequest);

    console.log("Form after reset:", getValues("experiences"));

    setRevertSnapshot(null);

    if (!deletedBulletsWarningInitialized.current) {
      const hasDeletedSourceBullets = resumeDetails.resume.experiences.some(
        (experience) =>
          experience.bullets.some((bullet) => bullet.isSourceDeleted),
      );

      setDeletedBulletsWarning(hasDeletedSourceBullets);

      deletedBulletsWarningInitialized.current = true;
    }
  }, [resumeDetails, reset, resumeId]);

  const {
    append: appendEducation,
    remove: removeEducation,
    update: updateEducation,
  } = useFieldArray({
    control,
    name: "education",
    keyName: "fieldId",
  });

  const { append: appendExperience, remove: removeExperience } = useFieldArray({
    control,
    name: "experiences",
    keyName: "fieldId",
  });

  const handleBulletReplace = (
    libraryExperienceId: number,
    currentLibraryBulletId: number,
    newLibraryBulletId: number,
  ) => {
    const experienceIndex = getExperienceIndex(libraryExperienceId);

    if (experienceIndex === -1) return;

    const path = `experiences.${experienceIndex}.bullets` as const;
    const bullets = getValues(path);

    const bulletIndex = bullets.findIndex(
      (bullet) => bullet.libraryBulletId === currentLibraryBulletId,
    );

    if (bulletIndex === -1) return;

    // Prevent selecting the same library bullet twice
    if (
      bullets.some(
        (bullet, index) =>
          index !== bulletIndex &&
          bullet.libraryBulletId === newLibraryBulletId,
      )
    ) {
      return;
    }

    setValue(
      `experiences.${experienceIndex}.bullets.${bulletIndex}.libraryBulletId`,
      newLibraryBulletId,
      { shouldDirty: true },
    );
  };

  const handleBulletAdd = (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => {
    const experienceIndex = getExperienceIndex(libraryExperienceId);
    if (experienceIndex === -1) return;

    const path = `experiences.${experienceIndex}.bullets` as const;
    const bullets = getValues(path);

    if (bullets.some((b) => b.libraryBulletId === libraryBulletId)) return;

    setValue(
      path,
      [
        ...bullets,
        {
          id: null,
          libraryBulletId,
          sortOrder: bullets.length + 1,
        },
      ],
      { shouldDirty: true },
    );
  };

  const handleBulletMoveUp = (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => {
    const experienceIndex = getExperienceIndex(libraryExperienceId);
    if (experienceIndex === -1) return;

    const path = `experiences.${experienceIndex}.bullets` as const;
    const bullets = [...getValues(path)].sort(
      (a, b) => a.sortOrder - b.sortOrder,
    );

    const bulletIndex = bullets.findIndex(
      (bullet) => bullet.libraryBulletId === libraryBulletId,
    );

    if (bulletIndex <= 0) return;

    [bullets[bulletIndex - 1], bullets[bulletIndex]] = [
      bullets[bulletIndex],
      bullets[bulletIndex - 1],
    ];

    setValue(
      path,
      bullets.map((bullet, index) => ({
        ...bullet,
        sortOrder: index + 1,
      })),
      { shouldDirty: true },
    );
  };

  const handleBulletMoveDown = (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => {
    const experienceIndex = getExperienceIndex(libraryExperienceId);
    if (experienceIndex === -1) return;

    const path = `experiences.${experienceIndex}.bullets` as const;
    const bullets = [...getValues(path)].sort(
      (a, b) => a.sortOrder - b.sortOrder,
    );

    const bulletIndex = bullets.findIndex(
      (bullet) => bullet.libraryBulletId === libraryBulletId,
    );

    if (bulletIndex === -1 || bulletIndex >= bullets.length - 1) return;

    [bullets[bulletIndex], bullets[bulletIndex + 1]] = [
      bullets[bulletIndex + 1],
      bullets[bulletIndex],
    ];

    setValue(
      path,
      bullets.map((bullet, index) => ({
        ...bullet,
        sortOrder: index + 1,
      })),
      { shouldDirty: true },
    );
  };

  const handleBulletRemove = (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => {
    const experienceIndex = getExperienceIndex(libraryExperienceId);
    if (experienceIndex === -1) return;

    const path = `experiences.${experienceIndex}.bullets` as const;

    const bullets = getValues(path);

    const updatedBullets = bullets
      .filter((bullet) => bullet.libraryBulletId !== libraryBulletId)
      .map((bullet, index) => ({
        ...bullet,
        sortOrder: index + 1,
      }));

    setValue(path, updatedBullets, { shouldDirty: true });
  };

  const handleAppendExperience = (libraryExperienceId: number) => {
    const experiences = getValues("experiences");

    const experience = experienceMapping.get(libraryExperienceId);

    appendExperience({
      id: null,
      libraryExperienceId,
      sortOrder: experiences.length + 1,
      librarySummaryId: experience?.summaries[0]?.librarySummaryId ?? null,
      bullets: [],
    });
  };

  const getExperienceIndex = (libraryExperienceId: number): number => {
    return getValues("experiences").findIndex(
      (experience) => experience.libraryExperienceId === libraryExperienceId,
    );
  };

  const handleAppendEducation = (libraryEducationId: number) => {
    appendEducation({
      id: null,
      libraryEducationId,
      sortOrder: (formValues?.education?.length ?? 0) + 1,
    });
  };

  const handleRemoveEducation = (index: number) => {
    removeEducation(index);
  };

  const handleUpdateEducation = (index: number, libraryEducationId: number) => {
    const current = formValues.education?.[index];

    if (!current) {
      return;
    }

    updateEducation(index, {
      id: current.id ?? null,
      libraryEducationId,
      sortOrder: current.sortOrder ?? index + 1,
    });
  };

  const handleRemoveExperience = (libraryExperienceId: number) => {
    const index = getValues("experiences").findIndex(
      (experience) => experience.libraryExperienceId === libraryExperienceId,
    );

    if (index === -1) return;

    removeExperience(index);
  };

  const handleTitleFontSizeChange = (size: number) => {
    setValue("appearance.titleFontSize", size, { shouldDirty: true });
  };

  const handleSectionHeaderFontSizeChange = (size: number) => {
    setValue("appearance.sectionHeaderFontSize", size, { shouldDirty: true });
  };

  const handleMainBodyFontSizeChange = (size: number) => {
    setValue("appearance.mainBodyFontSize", size, { shouldDirty: true });
  };

  const handleTopHeaderAlignmentChange = (
    alignment: "left" | "center" | "right",
  ) => {
    setValue("appearance.topHeaderAlignment", alignment, { shouldDirty: true });
  };

  // restore the version before the most recent save
  const handleRevert = async () => {
    if (!revertSnapshot) {
      return;
    }

    try {
      const previousVersion = structuredClone(revertSnapshot);

      const updatedDetails =
        await updateResumeMutation.mutateAsync(previousVersion);

      const revertedFormData = mapResumeToRequest(updatedDetails.resume);

      setSavedResume(updatedDetails.resume); // <-- add this

      lastSavedRef.current = structuredClone(revertedFormData);

      reset(revertedFormData);

      setRevertSnapshot(null);

      setIsSavedSuccessfully(true);
    } catch {
      setIsSavedSuccessfully(false);
    }
  };

  // undo unsaved changes
  const handleDiscardChanges = () => {
    if (!lastSavedRef.current) {
      return;
    }

    reset(structuredClone(lastSavedRef.current));
  };

  const orderSkills = (
    skills: SkillResponse[],
    experiences: ResolvedExperience[],
  ): SkillResponse[] => {
    const orderedSkills: SkillResponse[] = [];
    const usedSkillIds = new Set<number>();

    const addSkillsFromText = (text: string) => {
      const normalizedText = text.toLowerCase();

      for (const skill of skills) {
        if (usedSkillIds.has(skill.id)) continue;

        if (normalizedText.includes(skill.value.toLowerCase())) {
          orderedSkills.push(skill);
          usedSkillIds.add(skill.id);
        }
      }
    };

    const selectedExperiences = experiences
      .filter((exp) => exp.useInResume)
      .sort((a, b) => a.sortOrder - b.sortOrder);

    const employment = selectedExperiences.filter(
      (exp) => exp.type === "employment",
    );

    const projects = selectedExperiences.filter(
      (exp) => exp.type === "personalProject",
    );

    for (const company of employment) {
      const bullets = company.bullets
        .filter((bullet) => bullet.useInResume)
        .sort((a, b) => a.sortOrder - b.sortOrder);

      for (const bullet of bullets) {
        addSkillsFromText(bullet.value);
      }
    }

    for (const project of projects) {
      const bullets = project.bullets
        .filter((bullet) => bullet.useInResume)
        .sort((a, b) => a.sortOrder - b.sortOrder);

      const summary = project.summaries.find((s) => s.useInResume);

      for (const bullet of bullets) {
        addSkillsFromText(bullet.value);
      }

      addSkillsFromText(project.title ?? "");
      addSkillsFromText(summary?.value ?? "");
      addSkillsFromText(project.techStack ?? "");
    }

    // Append skills that weren't found in the resume content
    for (const skill of skills) {
      if (!usedSkillIds.has(skill.id)) {
        orderedSkills.push(skill);
      }
    }

    return orderedSkills;
  };

  const onSubmit = async (values: UpdateResumeRequest) => {
    try {
      const previousSavedVersion = lastSavedRef.current
        ? structuredClone(lastSavedRef.current)
        : null;

      const updatedDetails = await updateResumeMutation.mutateAsync(values);

      const updatedFormData = mapResumeToRequest(updatedDetails.resume);

      setRevertSnapshot(previousSavedVersion);

      setSavedResume(updatedDetails.resume); // <-- add this

      lastSavedRef.current = structuredClone(updatedFormData);

      reset(updatedFormData);

      setIsSavedSuccessfully(true);
    } catch {
      setIsSavedSuccessfully(false);
    }
  };

  const formValues = useWatch({
    control,
  });

  const selectedEducations = new Map(
    (formValues.education ?? [])
      .filter((education) => education.libraryEducationId != null)
      .map((education) => [education.libraryEducationId!, education]),
  );

  const selectedExperiences = new Map(
    (formValues.experiences ?? [])
      .filter((experience) => experience.libraryExperienceId != null)
      .map((experience) => [experience.libraryExperienceId!, experience]),
  );

  console.log("Form experiences:", formValues.experiences);

  console.log("Selected experience IDs:", [...selectedExperiences.keys()]);

  console.log("Library experience IDs:", [...experienceMapping.keys()]);

  if (isLoading) {
    return <div>Loading resume details...</div>;
  }

  if (error) {
    return <div>Error loading resume details: {error.message}</div>;
  }

  if (!baseResume) {
    return <h1>Resume not found</h1>;
  }

  const handleDownloadPdf = async () => {
    const pdf = await getResumePdf(resumeId);

    const url = URL.createObjectURL(pdf);

    const link = document.createElement("a");
    link.href = url;
    link.download = `${baseResume.resumeName}.pdf`;

    document.body.appendChild(link);
    link.click();
    link.remove();

    URL.revokeObjectURL(url);
  };

  const resolvedResume: ResolvedResume = {
    ...baseResume,
    education: [...educationMapping.values()].map((education) => {
      const selected = selectedEducations.get(education.libraryEducationId);

      return {
        ...education,
        resumeEducationId: selected?.id ?? null,
        sortOrder: selected?.sortOrder ?? 0,
        useInResume: !!selected,
      };
    }),

    experiences: [...experienceMapping.values()].map((experience) => {
      const selected = selectedExperiences.get(experience.libraryExperienceId);

      const selectedBullets = new Map(
        (selected?.bullets ?? [])
          .filter((bullet) => bullet.libraryBulletId != null)
          .map((bullet) => [bullet.libraryBulletId!, bullet]),
      );

      return {
        ...experience,
        resumeExperienceId: selected?.id ?? null,
        sortOrder: selected?.sortOrder ?? 0,
        useInResume: !!selected,

        summaries: experience.summaries.map((summary) => ({
          ...summary,
          useInResume:
            !!selected &&
            selected.librarySummaryId === summary.librarySummaryId,
        })),

        bullets: experience.bullets.map((bullet) => {
          const selectedBullet = selectedBullets.get(bullet.libraryBulletId);

          return {
            ...bullet,
            resumeBulletId: selectedBullet?.id ?? null,
            sortOrder: selectedBullet?.sortOrder ?? 0,
            useInResume: !!selectedBullet,
          };
        }),
      };
    }),

    appearance: {
      // Keep your existing appearance mapping here
      ...baseResume.appearance,
      ...formValues.appearance,
      id: baseResume.appearance.id,
    },
  };

  resolvedResume.skills = orderSkills(
    resolvedResume.skills ?? [],
    resolvedResume.experiences ?? [],
  );

  console.log("RESOLVED RESUME:", resolvedResume);

  const availableEducationButExcludingCurrent = resolvedResume.education.filter(
    (education) => !education.useInResume && education.type === "degree",
  );

  const availableCertificationsButExcludingCurrent =
    resolvedResume.education.filter(
      (education) =>
        !education.useInResume && education.type === "certification",
    );

  const availableProjectsButExcludingCurrent =
    resolvedResume.experiences.filter(
      (experience) =>
        !experience.useInResume && experience.type === "personalProject",
    );

  return (
    <div className="flex w-full min-w-0 items-start gap-6 max-[1600px]:flex-col">
      {/*Actual Resume */}
      <div className="flex flex-col gap-4 justify-center items-center w-full">
        {isSavedSuccessfully && (
          <Message
            type="success"
            message="Your resume has been saved successfully."
            onClose={() => setIsSavedSuccessfully(null)}
          />
        )}
        {isSavedSuccessfully === false && (
          <Message
            type="error"
            message="Failed to save your resume."
            onClose={() => setIsSavedSuccessfully(null)}
          />
        )}

        {deletedBulletsWarning && (
          <Message
            type="warning"
            message="Some bullets in this resume were deleted from your bullet library. They are marked with a red warning icon in the editor and will appear normally in the final resume. If you remove one from this resume, it cannot be restored unless you recreate it."
            onClose={() => setDeletedBulletsWarning(false)}
          />
        )}

        <div className="flex justify-center gap-4">
          <ButtonContainer
            hasStyle={true}
            isRelative={true}
            align="space-between"
          >
            {availableEducationButExcludingCurrent.length > 0 && (
              <DropdownButton
                label="Add Education"
                iconComponent={() => <Book size={20} />}
                styleType="filter"
                labelSlideOut={true}
              >
                <SearchDropdown
                  filteredActions={availableEducationButExcludingCurrent.map(
                    (education) => ({
                      label: education.institutionName,
                      onClick: () =>
                        handleAppendEducation(education.libraryEducationId),
                    }),
                  )}
                />
              </DropdownButton>
            )}
            {availableCertificationsButExcludingCurrent.length > 0 && (
              <DropdownButton
                label="Add Certification"
                iconComponent={() => <PatchCheck size={20} />}
                styleType="filter"
                labelSlideOut={true}
              >
                <Dropdown
                  actions={availableCertificationsButExcludingCurrent.map(
                    (certification) => ({
                      label: certification.institutionName,
                      onClick: () =>
                        handleAppendEducation(certification.libraryEducationId),
                    }),
                  )}
                />
              </DropdownButton>
            )}
            {availableProjectsButExcludingCurrent.length > 0 && (
              <DropdownButton
                label="Add Project"
                iconComponent={() => <Folder2Open size={20} />}
                styleType="filter"
                labelSlideOut={true}
              >
                <Dropdown
                  actions={availableProjectsButExcludingCurrent.map(
                    (project) => ({
                      label: project.title,
                      onClick: () =>
                        handleAppendExperience(project.libraryExperienceId),
                    }),
                  )}
                />
              </DropdownButton>
            )}
            <DropdownButton
              label={`Title (${formValues.appearance?.titleFontSize ?? 16}pt)`}
              iconComponent={() => <TypeH1 size={20} />}
              styleType="filter"
              labelSlideOut={true}
            >
              <Dropdown
                actions={[16, 17, 18, 19, 20].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleTitleFontSizeChange(size),
                }))}
              />
            </DropdownButton>
            <DropdownButton
              label={`Sections (${formValues.appearance?.sectionHeaderFontSize ?? 14}pt)`}
              iconComponent={() => <TypeH2 size={20} />}
              styleType="filter"
              labelSlideOut={true}
            >
              <Dropdown
                actions={[14, 15, 16, 17, 18, 19, 20].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleSectionHeaderFontSizeChange(size),
                }))}
              />
            </DropdownButton>
            <DropdownButton
              label={`Main (${formValues.appearance?.mainBodyFontSize ?? 10}pt)`}
              iconComponent={() => <Paragraph size={20} />}
              styleType="filter"
              labelSlideOut={true}
            >
              <Dropdown
                actions={[10, 11, 12].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleMainBodyFontSizeChange(size),
                }))}
              />
            </DropdownButton>
          </ButtonContainer>
        </div>
        <ScaledResumePreview>
          <div className="resume-preview">
            <ResumeEditProvider>
              <Resume
                resume={resolvedResume}
                onBulletReplace={handleBulletReplace}
                onBulletMoveUp={handleBulletMoveUp}
                onBulletMoveDown={handleBulletMoveDown}
                onBulletAdd={handleBulletAdd}
                onBulletRemove={handleBulletRemove}
                onRemoveEducation={handleRemoveEducation}
                onUpdateEducation={handleUpdateEducation}
                onRemoveExperience={handleRemoveExperience}
                onTopHeaderAlignmentChange={handleTopHeaderAlignmentChange}
              />
            </ResumeEditProvider>
          </div>
        </ScaledResumePreview>

        <div className="flex justify-end w-full gap-4 max-[1600px]:justify-center">
          <div className="flex gap-4">
            <button
              type="button"
              className="remove-btn"
              onClick={() => handleDiscardChanges()}
            >
              Discard Changes
            </button>
            <button type="button" className="remove-btn" onClick={handleRevert}>
              Revert Last Save
            </button>

            <button
              type="button"
              className="confirm-btn"
              onClick={handleDownloadPdf}
              disabled={updateResumeMutation.isPending}
            >
              Download PDF
            </button>

            <button
              type="button"
              className="confirm-btn"
              onClick={handleSubmit(onSubmit)}
              disabled={updateResumeMutation.isPending}
            >
              {updateResumeMutation.isPending ? "Saving..." : "Save Resume"}
            </button>
          </div>
        </div>
      </div>

      {/*Resume Summary */}
      <div className="w-full flex flex-col items-center gap-6">
        <Card className="w-full">
          <RatingBar rating={resumeDetails?.resume.aiScore ?? 0} />
        </Card>
        {/* AI Summary Section */}
        <Card className="w-full">
          <h3 className="primary-color text-xl">AI Summary</h3>
          <p>{resumeDetails?.aiAnalysis?.summary}</p>
        </Card>
        {/* Alternative Bullets Section */}
        <Card className="w-full">
          <h3 className="primary-color text-xl">Alternative Bullets</h3>
          <ul>
            {baseResume.experiences.map((company) =>
              company.bullets
                .map((bullet) => bullet.aiAlternativeValue)
                .filter((value) => value !== null)
                .map((value, index) => (
                  <ListItem key={index} content={value} />
                )),
            )}
          </ul>
        </Card>
        {/*Weaknesses and Strengths Sections*/}
        <Card className="w-full flex flex-col gap-6">
          {/*Strengths*/}
          <div>
            <h3 className="primary-color text-xl">Strengths</h3>
            <ul>
              {resumeDetails?.aiAnalysis?.strengths?.map((strength, index) => (
                <ListItem key={index} content={strength} />
              ))}
            </ul>
          </div>
          {/*Weaknesses*/}
          <div>
            <h3 className="primary-color text-xl">Weaknesses</h3>
            <ul>
              {resumeDetails?.aiAnalysis?.weaknesses?.map((weakness, index) => (
                <ListItem key={index} content={weakness} />
              ))}
            </ul>
          </div>
        </Card>
      </div>
    </div>
  );
};

export default ResumeDetailsPage;
