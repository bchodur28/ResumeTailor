import type { ResumeResponse } from "../../api/contracts/resumes/ResumeResponse";
import BulletList from "./BulletList";
import InnerSection from "./InnerSection";
import styles from "./Resume.module.css";
import Section from "./Section";
import TwoColumn from "./TwoColumn";

type ResumeProps = {
  resume: ResumeResponse;
};

const Resume = ({ resume }: ResumeProps) => {
  return (
    <>
      <div className={styles.resume}>
        <section className={styles.topHeader}>
          <h1>{resume.personName}</h1>
          <h2>{resume.profession}</h2>
          <p>{resume.location}</p>
          <ul className={`${styles.contactList} ${styles.mainFrontSize}`}>
            <li>{resume.phoneNumber}</li>
            {resume.personalLinks.map((link, index) => (
              <li key={index}>
                <a href={link.url}>{link.displayName}</a>
              </li>
            ))}
          </ul>
        </section>
        <Section title="COMPANIES" topPtSpacing={8}>
          {resume.companies.map((company, index) => {
            return (
              <InnerSection
                key={index}
                topPtSpacing={resume.companies.length - 1 === index ? 8 : 4}
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
                  items={company.bullets}
                  topListPtSpacing={8}
                  verticalItemPtSpacing={4}
                />
              </InnerSection>
            );
          })}
        </Section>
        <Section title="EDUCATION" topPtSpacing={8}>
          {resume.education.map((education, index) => (
            <InnerSection key={index} topPtSpacing={4}>
              <div className={`${styles.resumeTwoColumn} font-bold`}>
                <p>{education.schoolName}</p>
                <p>
                  {education.ended
                    ? education.ended
                    : education.started + " - Present"}
                </p>
              </div>
              <div className={styles.resumeTwoColumn}>
                <p>{education.degree}</p>
                <p>{education.major}</p>
              </div>
            </InnerSection>
          ))}
        </Section>
        <Section title="PROJECTS" topPtSpacing={8}>
          {resume.projects.map((project, index) => {
            return (
              <InnerSection key={index} topPtSpacing={4}>
                <TwoColumn
                  left={project.name}
                  right={project.started + " - " + (project.ended ?? "Present")}
                  isBold={true}
                />
                <InnerSection topPtSpacing={4}>
                  <p>{project.description}</p>
                </InnerSection>
                {project.techStack && project.techStack.length > 0 && (
                  <InnerSection topPtSpacing={8}>
                    <p>
                      <b>Tech Stack:</b> {project.techStack}
                    </p>
                  </InnerSection>
                )}

                {project.link && project.link.length > 0 && (
                  <InnerSection topPtSpacing={4}>
                    <p>
                      <b>Link:</b> <a href={project.link}>{project.link}</a>
                    </p>
                  </InnerSection>
                )}
              </InnerSection>
            );
          })}
        </Section>
      </div>
    </>
  );
};

export default Resume;
