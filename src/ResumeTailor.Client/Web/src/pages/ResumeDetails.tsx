import { useEffect, useState, useRef } from "react";
import { useFieldArray, useForm, useWatch } from "react-hook-form";
import type { ResumeDetailsForm } from "../models/forms/ResumeDetailsForm";
import type { ResumeEducationSelectionRequest } from "../api/contracts/resumes/ResumeEducationSelectionRequest";
import ListItem from "../components/ui/ListItem";
import { useParams } from "react-router-dom";
import { useResumeDetails } from "../hooks/useResumeDetails";
import { useProjects } from "../hooks/useProjects";
import Card from "../components/ui/Card";
import Resume from "../components/resume/Resume";
import RatingBar from "../components/ui/RatingBar";
import type { ResumeResponse } from "../api/contracts/resumes/ResumeResponse";
import type { EducationResponse } from "../api/contracts/education/EducationResponse";
import type { ProjectResponse } from "../api/contracts/projects/ProjectResponse";
import type { ResumeProjectSelectionRequest } from "../api/contracts/resumes/ResumeProjectSelectionRequest";
import { useEducation } from "../hooks/useEducation";
import type { ResumeCompanySelectionRequest } from "../api/contracts/resumes/ResumeCompanySelectionRequest";
import EditToolBar from "../components/resume/EditToolBar";
import EditToolBarButton from "../components/resume/EditToolBarButton";
import {
  Book,
  Folder2Open,
  TypeH2,
  TypeH1,
  Paragraph,
} from "react-bootstrap-icons";
import { useCompanyBullets } from "../hooks/useCompanyBullet";
import { ResumeEditProvider } from "../contexts/ResumeEditContext";
import type { SkillResponse } from "../api/contracts/accounts/SkillResponse";
import type { ResumeCompanyResult } from "../models/resumes/ResumeCompanyResult";
import { getResumePdf, updateResume } from "../api/resumeApi";
import { useUpdateResume } from "../hooks/useUpdateResume";
import Message from "../components/ui/Message";

const mapResumeToForm = (
  resume: ResumeResponse,
  resumeId: number,
): ResumeDetailsForm => ({
  name: resume.resumeName ?? "",

  companies: resume.companies.map((company, index) => ({
    id: company.selectionId,
    resumeId,
    companyId: company.companyId,
    sortOrder: index + 1,
    bullets: company.bullets.map((bullet, index) => ({
      id: bullet.id,
      sourceBulletId: bullet.sourceBulletId,
      value: bullet.value,
      alternativeValue: bullet.alternativeValue,
      sortOrder: bullet.sortOrder ?? index + 1,
      isSourceDeleted: bullet.isSourceDeleted,
    })),
  })),

  education: resume.education.map((education, index) => ({
    id: education.selectionId,
    educationId: education.id,
    sortOrder: index + 1,
  })),

  projects: resume.projects.map((project, index) => ({
    id: project.selectionId,
    projectId: project.id,
    sortOrder: index + 1,
  })),

  appearance: resume.appearance,
});

const ResumeDetails = () => {
  const { id } = useParams();
  const resumeId = Number(id);

  const { data: resumeDetails, isLoading, error } = useResumeDetails(resumeId);
  const { data: availableEducation = [] } = useEducation();
  const { data: availableProjects = [] } = useProjects();
  const { data: availableCompanyBullets = [] } = useCompanyBullets();
  const updateResumeMutation = useUpdateResume(resumeId);

  const lastSavedRef = useRef<ResumeDetailsForm | null>(null);
  const [revertSnapshot, setRevertSnapshot] =
    useState<ResumeDetailsForm | null>(null);

  const [savedResume, setSavedResume] = useState<ResumeResponse | null>(null);

  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const [deletedBulletsWarning, setDeletedBulletsWarning] = useState(false);
  const deletedBulletsWarningInitialized = useRef(false);

  const { handleSubmit, reset, control, setValue, getValues } =
    useForm<ResumeDetailsForm>({
      defaultValues: {
        name: "",
        companies: [] as ResumeCompanySelectionRequest[],
        education: [] as ResumeEducationSelectionRequest[],
        projects: [] as ResumeProjectSelectionRequest[],
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

    const formData: ResumeDetailsForm = mapResumeToForm(
      resumeDetails.resume,
      resumeId,
    );

    reset(formData);

    lastSavedRef.current = structuredClone(formData);

    setRevertSnapshot(null);

    if (!deletedBulletsWarningInitialized.current) {
      const hasDeletedSourceBullets = resumeDetails.resume.companies.some(
        (company) => company.bullets.some((bullet) => bullet.isSourceDeleted),
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

  const {
    append: appendProjects,
    remove: removeProjects,
    update: updateProjects,
  } = useFieldArray({
    control,
    name: "projects",
    keyName: "fieldId",
  });

  const handleBulletChange = (
    companyIndex: number,
    bulletIndex: number,
    sourceBulletId: number | null,
    value: string,
  ) => {
    setValue(
      `companies.${companyIndex}.bullets.${bulletIndex}.sourceBulletId`,
      sourceBulletId,
    );

    setValue(`companies.${companyIndex}.bullets.${bulletIndex}.value`, value);
  };

  const handleBulletAdd = (
    companyIndex: number,
    sourceBulletId: number,
    value: string,
  ) => {
    const bullets = getValues(`companies.${companyIndex}.bullets`);

    setValue(`companies.${companyIndex}.bullets`, [
      ...bullets,
      {
        id: null,
        sourceBulletId,
        value,
        alternativeValue: null,
        sortOrder: bullets.length + 1,
      },
    ]);
  };

  const handleBulletMoveUp = (companyIndex: number, bulletIndex: number) => {
    if (bulletIndex === 0) {
      return;
    }

    const bullets = getValues(`companies.${companyIndex}.bullets`);
    const updatedBullets = [...bullets];
    const [movedBullet] = updatedBullets.splice(bulletIndex, 1);
    updatedBullets.splice(bulletIndex - 1, 0, movedBullet);

    const reorderedBullets = updatedBullets.map((bullet, index) => ({
      ...bullet,
      sortOrder: index + 1,
    }));

    console.log("Reordered Bullets UP:", reorderedBullets);

    setValue(`companies.${companyIndex}.bullets`, reorderedBullets);
  };

  const handleBulletMoveDown = (companyIndex: number, bulletIndex: number) => {
    const bullets = getValues(`companies.${companyIndex}.bullets`);
    if (bulletIndex === bullets.length - 1) {
      return;
    }

    const updatedBullets = [...bullets];

    const [movedBullet] = updatedBullets.splice(bulletIndex, 1);

    updatedBullets.splice(bulletIndex + 1, 0, movedBullet);

    const reorderedBullets = updatedBullets.map((bullet, index) => ({
      ...bullet,
      sortOrder: index + 1,
    }));

    console.log("Reordered Bullets DOWN:", reorderedBullets);

    setValue(`companies.${companyIndex}.bullets`, reorderedBullets);
  };

  const handleAppendEducation = (educationId: number) => {
    appendEducation({
      id: null,
      educationId,
      sortOrder: (formValues?.education?.length ?? 0) + 1,
    });
  };

  const handleRemoveEducation = (index: number) => {
    removeEducation(index);
  };

  const handleUpdateEducation = (index: number, educationId: number) => {
    const current = formValues.education?.[index];

    if (!current) {
      return;
    }

    updateEducation(index, {
      id: current.id ?? null,
      educationId,
      sortOrder: current.sortOrder ?? index + 1,
    });
  };

  const handleAppendProject = (projectId: number) => {
    appendProjects({
      id: null,
      projectId,
      sortOrder: (formValues?.projects?.length ?? 0) + 1,
    });
  };

  const handleRemoveProject = (index: number) => {
    removeProjects(index);
  };

  const handleUpdateProject = (index: number, projectId: number) => {
    const current = formValues.projects?.[index];

    if (!current) {
      return;
    }

    updateProjects(index, {
      id: current.id ?? null,
      projectId,
      sortOrder: current.sortOrder ?? index + 1,
    });
    console.log("Updated Projects:", formValues.projects);
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

      const revertedFormData = mapResumeToForm(updatedDetails.resume, resumeId);

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
    companies: ResumeCompanyResult[],
    projects: ProjectResponse[],
  ) => {
    const orderedSkills: SkillResponse[] = [];
    const usedSkillIds = new Set<number>();

    const addSkillsFromText = (text: string) => {
      const normalizedText = text.toLowerCase();

      for (const skill of skills) {
        if (usedSkillIds.has(skill.id)) {
          continue;
        }

        if (normalizedText.includes(skill.value.toLowerCase())) {
          orderedSkills.push(skill);
          usedSkillIds.add(skill.id);
        }
      }
    };

    for (const company of companies) {
      const bullets = [...company.bullets].sort(
        (a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0),
      );

      for (const bullet of bullets) {
        addSkillsFromText(bullet.value);
      }
    }

    for (const project of projects) {
      addSkillsFromText(project.name);
      addSkillsFromText(project.description ?? "");
      addSkillsFromText(project.techStack ?? "");
    }

    // for (const skill of skills) {
    //   if (!usedSkillIds.has(skill.id)) {
    //     orderedSkills.push(skill);
    //   }
    // }

    return orderedSkills;
  };

  const onSubmit = async (values: ResumeDetailsForm) => {
    try {
      const previousSavedVersion = lastSavedRef.current
        ? structuredClone(lastSavedRef.current)
        : null;

      const updatedDetails = await updateResumeMutation.mutateAsync(values);

      const updatedFormData = mapResumeToForm(updatedDetails.resume, resumeId);

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

  if (isLoading) {
    return <div>Loading resume details...</div>;
  }

  if (error) {
    return <div>Error loading resume details: {error.message}</div>;
  }

  if (resumeDetails?.resume == null) {
    return <h1>Resume not found</h1>;
  }

  const handleDownloadPdf = async () => {
    const pdf = await getResumePdf(resumeId);

    const url = URL.createObjectURL(pdf);

    const link = document.createElement("a");
    link.href = url;
    link.download = `${resumeDetails.resume.resumeName}.pdf`;

    document.body.appendChild(link);
    link.click();
    link.remove();

    URL.revokeObjectURL(url);
  };

  const baseResume = savedResume ?? resumeDetails.resume;

  const previewResume: ResumeResponse = {
    ...baseResume,

    appearance: {
      id: baseResume.appearance.id,

      titleFontSize:
        formValues.appearance?.titleFontSize ??
        baseResume.appearance.titleFontSize,

      sectionHeaderFontSize:
        formValues.appearance?.sectionHeaderFontSize ??
        baseResume.appearance.sectionHeaderFontSize,

      mainBodyFontSize:
        formValues.appearance?.mainBodyFontSize ??
        baseResume.appearance.mainBodyFontSize,

      fontFamily:
        formValues.appearance?.fontFamily ?? baseResume.appearance.fontFamily,

      fontColor:
        formValues.appearance?.fontColor ?? baseResume.appearance.fontColor,

      topHeaderAlignment:
        formValues.appearance?.topHeaderAlignment ??
        baseResume.appearance.topHeaderAlignment,
    },

    companies: baseResume.companies.map((company) => {
      const formCompany = formValues.companies?.find(
        (x) => x.companyId === company.companyId,
      );

      return {
        ...company,
        bullets:
          formCompany?.bullets
            ?.map((formBullet) => {
              const originalBullet = company.bullets.find(
                (bullet) => bullet.id === formBullet.id,
              );

              return {
                selectionId: originalBullet?.selectionId ?? 0,
                id: formBullet.id ?? null,
                sourceBulletId: formBullet.sourceBulletId ?? null,
                value: formBullet.value ?? "",
                alternativeValue: formBullet.alternativeValue ?? null,
                sortOrder: formBullet.sortOrder ?? 0,
                isSourceDeleted: originalBullet?.isSourceDeleted ?? false,
              };
            })
            .toSorted((a, b) => a.sortOrder - b.sortOrder) ??
          [...company.bullets].sort(
            (a, b) => (a.sortOrder ?? 0) - (b.sortOrder ?? 0),
          ),
      };
    }),

    education:
      formValues.education
        ?.map((selection) =>
          availableEducation.find(
            (education: EducationResponse) =>
              education.id === selection.educationId,
          ),
        )
        .filter((x) => x != null) ?? [],

    projects:
      formValues.projects
        ?.map((selection) =>
          availableProjects.find(
            (project: ProjectResponse) => project.id === selection.projectId,
          ),
        )
        .filter((x) => x != null) ?? [],
  };

  const availableProjectsButExcludingCurrent = availableProjects.filter(
    (project) =>
      !previewResume.projects.some(
        (currentProject) => currentProject.id === project.id,
      ),
  );

  const availableEducationButExcludingCurrent = availableEducation.filter(
    (education) =>
      !previewResume.education.some(
        (currentEducation) => currentEducation.id === education.id,
      ),
  );

  const currentBulletIds = new Set(
    previewResume.companies
      .flatMap((company) => company.bullets)
      .map((bullet) => bullet.sourceBulletId)
      .filter((id) => id != null),
  );

  const availableCompanyBulletsButExcludingCurrent = availableCompanyBullets
    .map((company) => ({
      ...company,
      bullets: company.bullets?.filter(
        (bullet) => !currentBulletIds.has(bullet.id),
      ),
    }))
    .filter((company) => company.bullets && company.bullets.length > 0);

  const orderedSkills = orderSkills(
    previewResume.skills,
    previewResume.companies,
    previewResume.projects,
  );

  return (
    <div className="flex flex-col gap-6 items-center">
      <div className="flex items-start gap-6 ">
        {/*Actual Resume */}
        <div className="flex flex-col gap-4 justify-center items-center">
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

          <div className="flex justify-center w-full gap-4">
            <EditToolBar isRelative={true}>
              {availableEducationButExcludingCurrent.length > 0 && (
                <EditToolBarButton
                  label="Add Education"
                  icon={() => <Book size={20} />}
                  additionalActions={availableEducationButExcludingCurrent.map(
                    (education) => ({
                      label: education.schoolName,
                      onClick: () => handleAppendEducation(education.id),
                    }),
                  )}
                />
              )}
              {availableProjectsButExcludingCurrent.length > 0 && (
                <EditToolBarButton
                  label="Add Project"
                  icon={() => <Folder2Open size={20} />}
                  additionalActions={availableProjectsButExcludingCurrent.map(
                    (project) => ({
                      label: project.name,
                      onClick: () => handleAppendProject(project.id),
                    }),
                  )}
                />
              )}

              <EditToolBarButton
                label={`Title (${formValues.appearance?.titleFontSize ?? 16}pt)`}
                icon={() => <TypeH1 size={20} />}
                additionalActions={[16, 17, 18, 19, 20].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleTitleFontSizeChange(size),
                }))}
              />
              <EditToolBarButton
                label={`Sections (${formValues.appearance?.sectionHeaderFontSize ?? 14}pt)`}
                icon={() => <TypeH2 size={20} />}
                additionalActions={[14, 15, 16, 17, 18, 19, 20].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleSectionHeaderFontSizeChange(size),
                }))}
              />
              <EditToolBarButton
                label={`Main (${formValues.appearance?.mainBodyFontSize ?? 10}pt)`}
                icon={() => <Paragraph size={20} />}
                additionalActions={[10, 11, 12].map((size) => ({
                  label: `${size}pt`,
                  onClick: () => handleMainBodyFontSizeChange(size),
                }))}
              />
            </EditToolBar>
          </div>
          <div className="resume-preview">
            <ResumeEditProvider>
              <Resume
                resume={{ ...previewResume, skills: orderedSkills }}
                onBulletChange={handleBulletChange}
                onBulletMoveUp={handleBulletMoveUp}
                onBulletMoveDown={handleBulletMoveDown}
                onBulletAdd={handleBulletAdd}
                onRemoveEducation={handleRemoveEducation}
                onUpdateEducation={handleUpdateEducation}
                onRemoveProjects={handleRemoveProject}
                onUpdateProjects={handleUpdateProject}
                onTopHeaderAlignmentChange={handleTopHeaderAlignmentChange}
                availableProjects={availableProjectsButExcludingCurrent}
                availableEducation={availableEducationButExcludingCurrent}
                availableCompanyBullets={
                  availableCompanyBulletsButExcludingCurrent
                }
              />
            </ResumeEditProvider>
          </div>
          <div className="flex justify-end w-full gap-4">
            <div className="flex gap-4">
              <button
                type="button"
                className="remove-btn"
                onClick={() => handleDiscardChanges()}
              >
                Discard Changes
              </button>
              <button
                type="button"
                className="remove-btn"
                onClick={handleRevert}
              >
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
            <RatingBar rating={resumeDetails?.aiAnalysis?.score ?? 0} />
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
              {baseResume.companies.map((company) =>
                company.bullets
                  .map((bullet) => bullet.alternativeValue)
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
                {resumeDetails?.aiAnalysis?.strengths?.map(
                  (strength, index) => (
                    <ListItem key={index} content={strength} />
                  ),
                )}
              </ul>
            </div>
            {/*Weaknesses*/}
            <div>
              <h3 className="primary-color text-xl">Weaknesses</h3>
              <ul>
                {resumeDetails?.aiAnalysis?.weaknesses?.map(
                  (weakness, index) => (
                    <ListItem key={index} content={weakness} />
                  ),
                )}
              </ul>
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};

export default ResumeDetails;
