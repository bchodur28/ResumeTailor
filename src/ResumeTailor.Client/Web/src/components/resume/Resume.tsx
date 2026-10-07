import {
  Trash3,
  ArrowLeftRight,
  JustifyLeft,
  Justify,
  JustifyRight,
  Plus,
} from "react-bootstrap-icons";
import type { ResumeViewModel } from "../../models/forms/ResumeViewModel";
import type { ProjectResponse } from "../../api/contracts/projects/ProjectResponse";
import type { EducationResponse } from "../../api/contracts/education/EducationResponse";
import type { CompanyBulletsResponse } from "../../api/contracts/companies/CompanyBulletsResponse";
import BulletList from "./BulletList";
import EditableInnerSection from "./EditableInnerSection";
import InnerSection from "./InnerSection";
import styles from "./Resume.module.css";
import Section from "./Section";
import TwoColumn from "./TwoColumn";
import { getStateCode } from "../../data/states";

type ResumeProps = {
  resume: ResumeViewModel;
  onBulletChange: (
    companyIndex: number,
    bulletIndex: number,
    bulletId: number,
    value: string,
  ) => void;
  onBulletMoveUp: (companyIndex: number, bulletIndex: number) => void;
  onBulletMoveDown: (companyIndex: number, bulletIndex: number) => void;
  onBulletAdd: (companyIndex: number, bulletId: number, value: string) => void;

  onRemoveEducation: (index: number) => void;
  onUpdateEducation: (index: number, educationId: number) => void;

  onRemoveProjects: (index: number) => void;
  onUpdateProjects: (index: number, projectId: number) => void;

  onTopHeaderAlignmentChange: (alignment: "left" | "center" | "right") => void;

  availableProjects: ProjectResponse[];
  availableEducation: EducationResponse[];
  availableCompanyBullets: CompanyBulletsResponse[];
};

const Resume = ({
  resume,
  onBulletChange,
  onBulletMoveUp,
  onBulletMoveDown,
  onBulletAdd,
  onRemoveEducation,
  onUpdateEducation,
  onRemoveProjects,
  onUpdateProjects,
  onTopHeaderAlignmentChange,
  availableProjects,
  availableEducation,
  availableCompanyBullets,
}: ResumeProps) => {
  const headerAlignment = {
    left: "flex-start",
    center: "center",
    right: "flex-end",
  }[resume.appearance.topHeaderAlignment];

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
            <p style={{ fontSize: "11pt" }}>
              {resume.location.split(",")[0].trim() +
                ", " +
                getStateCode(resume.location.split(",")[1].trim())}
            </p>
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
        <Section title="SKILLS" topPtSpacing={8}>
          <InnerSection topPtSpacing={4} additionalClassName="flex gap-2">
            <p>{resume.skills.map((skill) => skill.value).join(", ")}</p>
          </InnerSection>
        </Section>
        <Section title="COMPANIES" topPtSpacing={8}>
          {resume.companies.map((company, companyIndex) => {
            return (
              <EditableInnerSection
                editorId={`company:${company.companyId}`}
                key={companyIndex}
                topPtSpacing={companyIndex === 0 ? 4 : 8}
                actions={[
                  {
                    label: "Add Bullet",
                    icon: () => <Plus size={20} />,
                    additionalActions: availableCompanyBullets
                      .filter((cb) => cb.companyId === company.companyId)
                      .flatMap((cb) =>
                        cb.bullets.map((bullet) => ({
                          label: bullet.value,
                          onClick: () =>
                            onBulletAdd(companyIndex, bullet.id, bullet.value),
                        })),
                      ),
                  },
                ]}
              >
                <TwoColumn
                  left={company.name}
                  right={company.started + " - " + (company.ended ?? "Present")}
                  isBold={true}
                />
                <TwoColumn
                  left={company.title}
                  right={company.location ?? "Remote"}
                />
                <BulletList
                  bullets={company.bullets}
                  topListPtSpacing={8}
                  verticalItemPtSpacing={4}
                  companyIndex={companyIndex}
                  onBulletChange={onBulletChange}
                  onBulletMoveUp={onBulletMoveUp}
                  onBulletMoveDown={onBulletMoveDown}
                  availableBullets={
                    availableCompanyBullets.find(
                      (cb) => cb.companyId === company.companyId,
                    )?.bullets ?? []
                  }
                />
              </EditableInnerSection>
            );
          })}
        </Section>
        {resume.education.length > 0 && (
          <Section title="EDUCATION" topPtSpacing={8}>
            {resume.education.map((education, index) => (
              <EditableInnerSection
                editorId={`education:${education.id}`}
                key={index}
                topPtSpacing={index === 0 ? 4 : 8}
                actions={[
                  {
                    onClick: () => onRemoveEducation(index),
                    label: "Remove",
                    icon: () => <Trash3 size={20} className="danger-color" />,
                    labelClassName: "danger-color",
                  },
                  ...(availableEducation.length === 0
                    ? []
                    : [
                        {
                          label: "Replace",
                          icon: () => <ArrowLeftRight size={20} />,
                          additionalActions: availableEducation.map(
                            (education) => ({
                              label: education.schoolName,
                              onClick: () =>
                                onUpdateEducation(index, education.id),
                            }),
                          ),
                        },
                      ]),
                ]}
              >
                <TwoColumn
                  left={education.schoolName}
                  right={
                    education.ended
                      ? education.ended
                      : education.started + " - Present"
                  }
                />
                <TwoColumn left={education.degree} right={education.major} />
              </EditableInnerSection>
            ))}
          </Section>
        )}
        {resume.projects.length > 0 && (
          <Section title="PROJECTS" topPtSpacing={8}>
            {resume.projects.map((project, index) => {
              return (
                <EditableInnerSection
                  editorId={`project:${project.id}`}
                  key={index}
                  topPtSpacing={index === 0 ? 4 : 8}
                  actions={[
                    {
                      onClick: () => onRemoveProjects(index),
                      label: "Remove",
                      icon: () => <Trash3 size={20} className="danger-color" />,
                      labelClassName: "danger-color",
                    },
                    ...(availableProjects.length === 0
                      ? []
                      : [
                          {
                            label: "Replace",
                            icon: () => <ArrowLeftRight size={20} />,
                            additionalActions: availableProjects.map(
                              (project) => ({
                                label: project.name,
                                onClick: () =>
                                  onUpdateProjects(index, project.id),
                              }),
                            ),
                          },
                        ]),
                  ]}
                >
                  <InnerSection key={index} topPtSpacing={0}>
                    <TwoColumn
                      left={project.name}
                      right={
                        project.started + " - " + (project.ended ?? "Present")
                      }
                      isBold={true}
                    />
                    <InnerSection topPtSpacing={4}>
                      <p className="pl-4">{project.description}</p>
                    </InnerSection>
                    {project.techStack && project.techStack.length > 0 && (
                      <InnerSection topPtSpacing={4}>
                        <p className="pl-4">
                          <b>Tech Stack:</b> {project.techStack}
                        </p>
                      </InnerSection>
                    )}

                    {project.link && project.link.length > 0 && (
                      <InnerSection topPtSpacing={4}>
                        <p className="pl-4">
                          <b>Link:</b> <a href={project.link}>{project.link}</a>
                        </p>
                      </InnerSection>
                    )}
                  </InnerSection>
                </EditableInnerSection>
              );
            })}
          </Section>
        )}
      </div>
    </>
  );
};

export default Resume;
