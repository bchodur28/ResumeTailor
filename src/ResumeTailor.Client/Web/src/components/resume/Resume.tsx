import {
  Trash3,
  ArrowLeftRight,
  JustifyLeft,
  Justify,
  JustifyRight,
  Plus,
  EyeSlash,
  Eye,
} from "react-bootstrap-icons";
import { useDateFormat } from "../../hooks/useDateFormat";
import BulletList from "./BulletList";
import EditableInnerSection from "./EditableInnerSection";
import InnerSection from "./InnerSection";
import styles from "./Resume.module.css";
import Section from "./Section";
import TwoColumn from "./TwoColumn";
import { getStateCode } from "../../data/states";
import type { ResolvedResume } from "../../models/resumes/ResolvedResume";

type ResumeProps = {
  resume: ResolvedResume;
  onBulletReplace: (
    libraryExperienceId: number,
    currentBulletId: number,
    newLibraryBulletId: number,
  ) => void;
  onBulletMoveUp: (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => void;
  onBulletMoveDown: (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => void;
  onBulletAdd: (libraryExperienceId: number, libraryBulletId: number) => void;
  onBulletRemove: (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => void;

  onRemoveEducation: (index: number) => void;
  onUpdateEducation: (index: number, educationId: number) => void;

  onRemoveExperience: (libraryExperienceId: number) => void;

  onTopHeaderAlignmentChange: (alignment: "left" | "center" | "right") => void;
  onHideCertificationIssuerChange: (hide: boolean) => void;
  hideCertificationIssuer: boolean;
};

const Resume = ({
  resume,
  onBulletReplace,
  onBulletMoveUp,
  onBulletMoveDown,
  onBulletAdd,
  onBulletRemove,
  onRemoveEducation,
  onUpdateEducation,
  onRemoveExperience,
  onTopHeaderAlignmentChange,
  onHideCertificationIssuerChange,
  hideCertificationIssuer,
}: ResumeProps) => {
  const headerAlignment = {
    left: "flex-start",
    center: "center",
    right: "flex-end",
  }[resume.appearance.topHeaderAlignment];

  const { formatDate } = useDateFormat();

  const selectedExperiences = resume.experiences
    .filter((exp) => exp.useInResume && exp.type === "employment")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  console.table(
    resume.experiences.map((exp) => ({
      id: exp.libraryExperienceId,
      title: exp.title,
      type: exp.type,
      useInResume: exp.useInResume,
      sortOrder: exp.sortOrder,
      bullets: exp.bullets.length,
    })),
  );

  const selectedEducation = resume.education
    .filter((edu) => edu.useInResume && edu.type === "degree")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const availableEducation = resume.education
    .filter((edu) => !edu.useInResume && edu.type === "degree")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const selectedCertifications = resume.education
    .filter((edu) => edu.useInResume && edu.type === "certification")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const availableCertifications = resume.education
    .filter((edu) => !edu.useInResume && edu.type === "certification")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const selectedProjects = resume.experiences
    .filter(
      (project) => project.useInResume && project.type === "personalProject",
    )
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const availableProjects = resume.experiences
    .filter((exp) => !exp.useInResume && exp.type === "personalProject")
    .sort((a, b) => a.sortOrder - b.sortOrder);

  const [city, state] = (resume.location ?? "").split(",");

  const formattedLocation = [
    city?.trim(),
    state ? getStateCode(state.trim()) : null,
  ]
    .filter(Boolean)
    .join(", ");

  return (
    <>
      <div
        className={styles.resume}
        style={
          {
            "--resume-title-font-size": `${resume.appearance.titleFontSize}pt`,
            "--resume-section-font-size": `${resume.appearance.sectionHeaderFontSize}pt`,
            "--resume-main-font-size": `${resume.appearance.mainBodyFontSize}pt`,
            "--resume-font-family": resume.appearance.fontFamily,
            "--resume-primary-color": resume.appearance.fontColor,
            "--resume-top-header-alignment": headerAlignment,
          } as React.CSSProperties
        }
      >
        <EditableInnerSection
          editorId="top-header"
          topPtSpacing={0}
          actions={[
            {
              label: "Align Left",
              icon: () => <JustifyLeft size={20} />,
              onClick: () => onTopHeaderAlignmentChange("left"),
            },
            {
              label: "Align Center",
              icon: () => <Justify size={20} />,
              onClick: () => onTopHeaderAlignmentChange("center"),
            },
            {
              label: "Align Right",
              icon: () => <JustifyRight size={20} />,
              onClick: () => onTopHeaderAlignmentChange("right"),
            },
          ]}
        >
          <div className={styles.topHeader}>
            <h1>{resume.personName}</h1>
            <p className={styles.personProfession}>{resume.profession}</p>
            <p style={{ fontSize: "11pt" }}>{formattedLocation}</p>
            <ul className={`${styles.contactList}`}>
              <li>{resume.email}</li>
              <li>{resume.phoneNumber}</li>
              {resume.personalLinks.map((link, index) => (
                <li key={index}>
                  <a href={link.url}>{link.displayName}</a>
                </li>
              ))}
            </ul>
          </div>
        </EditableInnerSection>

        {resume.summary && (
          <Section title="SUMMARY" topPtSpacing={8}>
            <InnerSection topPtSpacing={4} additionalClassName="flex gap-2">
              <p>{resume.summary}</p>
            </InnerSection>
          </Section>
        )}
        <Section title="SKILLS" topPtSpacing={8}>
          <InnerSection topPtSpacing={4} additionalClassName="flex gap-2">
            <p>{resume.skills.map((skill) => skill.value).join(", ")}</p>
          </InnerSection>
        </Section>
        <Section title="PROFESSIONAL EXPERIENCE" topPtSpacing={8}>
          {selectedExperiences.map((experience, companyIndex) => {
            const selectedBullets = experience.bullets
              .filter((bullet) => bullet.useInResume)
              .sort((a, b) => a.sortOrder - b.sortOrder);

            const availableBullets = experience.bullets.filter(
              (bullet) => !bullet.useInResume,
            );
            return (
              <EditableInnerSection
                editorId={`company:${experience.libraryExperienceId}`}
                key={companyIndex}
                topPtSpacing={companyIndex === 0 ? 4 : 8}
                actions={[
                  {
                    label: "Add Bullet",
                    icon: () => <Plus size={20} />,
                    additionalActions: availableBullets.map((bullet) => ({
                      label: bullet.value,
                      onClick: () =>
                        onBulletAdd(
                          experience.libraryExperienceId,
                          bullet.libraryBulletId,
                        ),
                    })),
                  },
                ]}
              >
                <TwoColumn
                  left={experience.organization ?? ""}
                  right={
                    formatDate(experience.started) +
                    " - " +
                    (experience.ended
                      ? formatDate(experience.ended)
                      : "Present")
                  }
                  isBold={true}
                />
                <TwoColumn
                  left={experience.title}
                  right={experience.location ?? "Remote"}
                />
                <BulletList
                  bullets={selectedBullets}
                  topListPtSpacing={8}
                  verticalItemPtSpacing={4}
                  companyIndex={companyIndex}
                  libraryExperienceId={experience.libraryExperienceId}
                  onBulletReplace={onBulletReplace}
                  onBulletMoveUp={onBulletMoveUp}
                  onBulletMoveDown={onBulletMoveDown}
                  onBulletRemove={onBulletRemove}
                  availableBullets={availableBullets}
                />
              </EditableInnerSection>
            );
          })}
        </Section>
        {selectedEducation.length > 0 && (
          <Section title="EDUCATION" topPtSpacing={8}>
            {selectedEducation.map((education, index) => (
              <EditableInnerSection
                editorId={`education:${education.resumeEducationId}`}
                key={index}
                topPtSpacing={index === 0 ? 4 : 8}
                actions={[
                  {
                    onClick: () => onRemoveEducation(index),
                    label: "Remove",
                    icon: () => <Trash3 size={20} className="danger-color" />,
                  },
                  ...(availableEducation.length === 0
                    ? []
                    : [
                        {
                          label: "Replace",
                          icon: () => <ArrowLeftRight size={20} />,
                          additionalActions: availableEducation.map(
                            (education) => ({
                              label: education.institutionName,
                              onClick: () =>
                                onUpdateEducation(
                                  index,
                                  education.libraryEducationId,
                                ),
                            }),
                          ),
                        },
                      ]),
                ]}
              >
                <TwoColumn
                  left={education.institutionName}
                  right={
                    education.ended
                      ? formatDate(education.ended)
                      : formatDate(education.started) + " - Present"
                  }
                  isBold
                />
                <TwoColumn
                  left={education.qualification}
                  right={education.fieldOfStudy ?? ""}
                />
              </EditableInnerSection>
            ))}
          </Section>
        )}
        {selectedCertifications.length > 0 && (
          <Section title="CERTIFICATIONS" topPtSpacing={8}>
            {selectedCertifications.map((certification, index) => (
              <EditableInnerSection
                editorId={`certification:${certification.resumeEducationId}`}
                key={index}
                topPtSpacing={index === 0 ? 4 : 8}
                actions={[
                  {
                    onClick: () => onRemoveEducation(index),
                    label: "Remove",
                    icon: () => <Trash3 size={20} className="danger-color" />,
                  },
                  {
                    onClick: () =>
                      onHideCertificationIssuerChange(!hideCertificationIssuer),
                    label: hideCertificationIssuer
                      ? "Show Certification Issuer"
                      : "Hide Certification Issuer",
                    icon: () =>
                      hideCertificationIssuer ? (
                        <Eye size={20} />
                      ) : (
                        <EyeSlash size={20} />
                      ),
                  },
                  ...(availableCertifications.length === 0
                    ? []
                    : [
                        {
                          label: "Replace",
                          icon: () => <ArrowLeftRight size={20} />,
                          additionalActions: availableCertifications.map(
                            (certification) => ({
                              label: certification.institutionName,
                              onClick: () =>
                                onUpdateEducation(
                                  index,
                                  certification.libraryEducationId,
                                ),
                            }),
                          ),
                        },
                      ]),
                ]}
              >
                <TwoColumn
                  left={certification.qualification}
                  right={
                    certification.ended
                      ? formatDate(certification.ended)
                      : formatDate(certification.started) + " - Present"
                  }
                  isBold
                />
                {!hideCertificationIssuer && (
                  <TwoColumn
                    left={certification.institutionName}
                    right={certification.fieldOfStudy ?? ""}
                  />
                )}
              </EditableInnerSection>
            ))}
          </Section>
        )}
        {/* Projects Section */}
        {selectedProjects.length > 0 && (
          <Section title="PROJECTS" topPtSpacing={8}>
            {selectedProjects.map((project, index) => (
              <EditableInnerSection
                key={project.libraryExperienceId}
                editorId={`project:${project.libraryExperienceId}`}
                topPtSpacing={index === 0 ? 4 : 8}
                actions={[
                  {
                    label: "Remove",
                    icon: () => <Trash3 size={20} className="danger-color" />,
                    onClick: () =>
                      onRemoveExperience(project.libraryExperienceId),
                    releaseLockOnClick: true,
                  },
                ]}
              >
                <InnerSection topPtSpacing={0}>
                  <TwoColumn
                    left={project.title}
                    right={`${formatDate(project.started)} - ${project.ended ? formatDate(project.ended) : "Present"}`}
                    isBold={true}
                  />

                  {project.summaries
                    .filter((summary) => summary.useInResume)
                    .map((summary) => (
                      <InnerSection
                        key={summary.librarySummaryId}
                        topPtSpacing={4}
                      >
                        <p className="pl-4">{summary.value}</p>
                      </InnerSection>
                    ))}

                  {project.techStack && (
                    <InnerSection topPtSpacing={4}>
                      <p className="pl-4">
                        <b>Tech Stack:</b> {project.techStack}
                      </p>
                    </InnerSection>
                  )}

                  {project.link && (
                    <InnerSection topPtSpacing={4}>
                      <p className="pl-4">
                        <b>Link:</b> <a href={project.link}>{project.link}</a>
                      </p>
                    </InnerSection>
                  )}
                </InnerSection>
              </EditableInnerSection>
            ))}
          </Section>
        )}
      </div>
    </>
  );
};

export default Resume;
