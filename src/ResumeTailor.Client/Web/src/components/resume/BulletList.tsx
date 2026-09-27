import type { ResumeBulletResult } from "../../models/resumes/ResumeBulletResult";
import styles from "./Resume.module.css";

type BulletListProps = {
  items: ResumeBulletResult[];
  topListPtSpacing: number;
  verticalItemPtSpacing: number;
};

const BulletList = ({
  items,
  topListPtSpacing = 1,
  verticalItemPtSpacing = 1,
}: BulletListProps) => {
  return (
    <ul
      className={styles.resumeBulletList}
      style={{
        paddingTop: `${topListPtSpacing}pt`,
        gap: `${verticalItemPtSpacing}pt`,
      }}
    >
      {items.map((item, index) => (
        <li key={index}>{item.value}</li>
      ))}
    </ul>
  );
};

export default BulletList;
