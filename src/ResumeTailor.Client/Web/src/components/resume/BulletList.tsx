import styles from "./Resume.module.css";

type BulletListProps = {
  items: string[];
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
        <li key={index}>{item}</li>
      ))}
    </ul>
  );
};

export default BulletList;
