import type { ResolvedBullet } from "../../models/Experience/ResolvedBullets";
import type { ResumeBulletViewModel } from "../../models/forms/ResumeViewModel";
import EditableBullet from "./EditableBullet";
import styles from "./Resume.module.css";

type BulletListProps = {
  bullets: ResolvedBullet[];
  availableBullets: ResolvedBullet[];
  topListPtSpacing: number;
  verticalItemPtSpacing: number;
  companyIndex: number;
  libraryExperienceId: number;
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
  onBulletRemove: (
    libraryExperienceId: number,
    libraryBulletId: number,
  ) => void;
};

const BulletList = ({
  bullets,
  availableBullets,
  topListPtSpacing = 1,
  verticalItemPtSpacing = 1,
  companyIndex,
  libraryExperienceId,
  onBulletReplace,
  onBulletMoveUp,
  onBulletMoveDown,
  onBulletRemove,
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
            item.resumeBulletId != null
              ? `resume-${item.resumeBulletId}`
              : `library-${item.libraryBulletId}`
          }
          item={item}
          additionalBullets={availableBullets}
          companyIndex={companyIndex}
          bulletIndex={index}
          onBulletReplace={(newLibraryBulletId) =>
            onBulletReplace(
              libraryExperienceId,
              item.libraryBulletId,
              newLibraryBulletId,
            )
          }
          onBulletMoveUp={() =>
            onBulletMoveUp(libraryExperienceId, item.libraryBulletId)
          }
          onBulletMoveDown={() =>
            onBulletMoveDown(libraryExperienceId, item.libraryBulletId)
          }
          onBulletRemove={() =>
            onBulletRemove(libraryExperienceId, item.libraryBulletId)
          }
          isLastBullet={index === bullets.length - 1}
          isFirstBullet={index === 0}
        />
      ))}
    </ul>
  );
};

export default BulletList;
