import type { BulletResponse } from "../../api/contracts/bullets/BulletResponse";
import type { ResumeBulletViewModel } from "../../models/forms/ResumeViewModel";
import EditableBullet from "./EditableBullet";
import styles from "./Resume.module.css";

type BulletListProps = {
  bullets: ResumeBulletViewModel[];
  availableBullets: BulletResponse[];
  topListPtSpacing: number;
  verticalItemPtSpacing: number;
  companyIndex: number;
  onBulletChange: (
    companyIndex: number,
    bulletIndex: number,
    bulletId: number,
    value: string,
  ) => void;
  onBulletMoveUp: (companyIndex: number, bulletIndex: number) => void;
  onBulletMoveDown: (companyIndex: number, bulletIndex: number) => void;
};

const BulletList = ({
  bullets,
  availableBullets,
  topListPtSpacing = 1,
  verticalItemPtSpacing = 1,
  companyIndex,
  onBulletChange,
  onBulletMoveUp,
  onBulletMoveDown,
}: BulletListProps) => {
  return (
    <ul
      className={styles.resumeBulletList}
      style={{
        paddingTop: `${topListPtSpacing}pt`,
        gap: `${verticalItemPtSpacing}pt`,
      }}
    >
      {bullets.map((item, index) => (
        <EditableBullet
          key={
            item.id != null
              ? `resume-${item.id}`
              : item.sourceBulletId != null
                ? `bullet-${item.sourceBulletId}`
                : `selection-${item.id}`
          }
          item={item}
          additionalBullets={availableBullets}
          companyIndex={companyIndex}
          bulletIndex={index}
          onBulletChange={onBulletChange}
          onBulletMoveUp={() => onBulletMoveUp(companyIndex, index)}
          onBulletMoveDown={() => onBulletMoveDown(companyIndex, index)}
          isLastBullet={index === bullets.length - 1}
          isFirstBullet={index === 0}
        />
      ))}
    </ul>
  );
};

export default BulletList;
