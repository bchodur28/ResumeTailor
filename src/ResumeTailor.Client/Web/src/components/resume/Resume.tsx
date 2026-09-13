import {
  cornerstoneBullets,
  buildertrendBullets,
} from "../../data/mockBullets";
import type { CompanyGeneratedResume } from "../../models/resume/CompanyGeneratedResume";
import BulletList from "./BulletList";
import InnerSection from "./InnerSection";
import styles from "./Resume.module.css";
import Section from "./Section";
import TwoColumn from "./TwoColumn";

type ResumeProps = {
  resume: CompanyGeneratedResume;
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
            <li>{resume.personalSite1}</li>
            <li>{resume.personalSite2}</li>
            <li>{resume.personalSite3}</li>
          </ul>
        </section>
        <Section title="SKILLS" topPtSpacing={8}>
          <InnerSection topPtSpacing={4}>
            <p>{resume.skills.join(", ")}</p>
          </InnerSection>
        </Section>
        <Section title="EXPERIENCE" topPtSpacing={8}>
          {resume.experience &&
            resume.experience.length > 0 &&
            resume.experience.map((exp, index) => {
              return (
                <InnerSection key={index} topPtSpacing={4}>
                  <TwoColumn
                    left={exp.name}
                    right={exp.workingStatus}
                    isBold={true}
                  />
                  <TwoColumn left={exp.position} right={exp.location} />
                  <BulletList
                    items={exp.bullets}
                    topListPtSpacing={8}
                    verticalItemPtSpacing={4}
                  />
                </InnerSection>
              );
            })}
        </Section>
        <Section title="EDUCATION" topPtSpacing={8}>
          <InnerSection topPtSpacing={4}>
            <div className={`${styles.resumeTwoColumn} font-bold`}>
              <p>{resume.college}</p>
              <p>{resume.collegeStatus}</p>
            </div>
            <div className={styles.resumeTwoColumn}>
              <p>{resume.degree}</p>
              <p>{resume.major}</p>
            </div>
          </InnerSection>
        </Section>
        <Section title="PROJECTS" topPtSpacing={8}>
          {resume.projects &&
            resume.projects.length > 0 &&
            resume.projects.map((project, index) => {
              return (
                <InnerSection key={index} topPtSpacing={4}>
                  <TwoColumn
                    left={project.name}
                    right={project.status}
                    isBold={true}
                  />
                  <InnerSection topPtSpacing={4}>
                    <p>{project.description}</p>
                  </InnerSection>
                  {project.techStack && project.techStack.length > 0 && (
                    <InnerSection topPtSpacing={4}>
                      <p>
                        <b>Tech stack:</b> {project.techStack.join(", ")}.
                      </p>
                    </InnerSection>
                  )}
                  {project.link && (
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
