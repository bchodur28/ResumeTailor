import React from "react";
import styles from "./Resume.module.css";

interface SectionProps {
  title: string;
  topPtSpacing: number;
  children: React.ReactNode;
}

const Section: React.FC<SectionProps> = ({ title, topPtSpacing, children }) => {
  return (
    <div
      className={styles.resumeSection}
      style={{
        paddingTop: `${topPtSpacing}pt`,
      }}
    >
      <h2>{title}</h2>
      <div className={styles.resumeSectionContent}>{children}</div>
    </div>
  );
};

export default Section;
