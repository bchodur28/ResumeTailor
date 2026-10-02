import type { BulletResponse } from "../../api/contracts/bullets/BulletResponse";
import type { CompanyBulletsResponse } from "../../api/contracts/companies/CompanyBulletsResponse";
import type { ResumeBulletResult } from "../../models/resumes/ResumeBulletResult";
import EditableBullet from "./EditableBullet";
import styles from "./Resume.module.css";

type BulletListProps = {
  bullets: ResumeBulletResult[];
  availableBullets: BulletResponse[];
  topListPtSpacing: number;
  verticalItemPtSpacing: number;
  companyIndex: number;
  onBulletChange: (
    companyIndex: number,
    bulletIndex: number,
    sourceBulletId: number | null,
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
                ? `source-${item.sourceBulletId}`
                : `selection-${item.selectionId}`
          }
          item={item}
          additonalBullets={availableBullets}
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
