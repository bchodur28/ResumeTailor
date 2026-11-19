import { useEffect, useState } from "react";
import {
  ArrowUp,
  ArrowDown,
  ArrowRepeat,
  Trash3,
  ArrowLeftRight,
  ExclamationTriangle,
} from "react-bootstrap-icons";
import type { ResolvedBullet } from "../../models/Experience/ResolvedBullets";
import styles from "./Resume.module.css";
import { useResumeEdit } from "../../contexts/ResumeEditContext";
import ButtonContainer from "../ui/buttons/ButtonContainer";
import Button from "../ui/buttons/Button";
import SearchDropdown from "../ui/dropdowns/SearchableDropdownActions";

type EditableBulletProps = {
  item: ResolvedBullet;
  additionalBullets: ResolvedBullet[];
  companyIndex: number;
  bulletIndex: number;
  isLastBullet: boolean;
  isFirstBullet: boolean;

  onBulletReplace: (newLibraryBulletId: number) => void;

  onBulletMoveUp: () => void;
  onBulletMoveDown: () => void;
  onBulletRemove: () => void;
};

const EditableBullet = ({
  item,
  additionalBullets,
  companyIndex,
  bulletIndex,
  isLastBullet,
  isFirstBullet,
  onBulletReplace,
  onBulletMoveUp,
  onBulletMoveDown,
  onBulletRemove,
}: EditableBulletProps) => {
  const { activeEditorId, toggleEditor, closeEditor } = useResumeEdit();

  const bulletId = item.resumeBulletId ?? item.libraryBulletId ?? bulletIndex;

  const editorId = `bullet:${companyIndex}:${bulletId}`;

  const isEditing = activeEditorId === editorId;

  const isLocked = activeEditorId !== null && activeEditorId !== editorId;

  const hasAlternative = item.aiAlternativeValue !== null;

  const [isReplaceOpen, setIsReplaceOpen] = useState(false);

  useEffect(() => {
    if (!isEditing) {
      setIsReplaceOpen(false);
    }
  }, [isEditing]);

  const handleBulletSwapWithAlternative = (alternativeValue: string) => {
    // onBulletReplace(
    //   companyIndex,
    //   bulletIndex,
    //   item.libraryBulletId,
    //   alternativeValue,
    // );
  };

  return (
    <li
      data-resume-editor-active={isEditing}
      className={`${styles.editableBulletContainer} ${
        isEditing ? styles.editableBulletContainerActive : ""
      }`}
    >
      <button
        type="button"
        aria-disabled={isLocked}
        className={`${styles.bulletListItem} ${
          isEditing ? styles.bulletListItemActive : ""
        } ${isLocked ? styles.editableControlLocked : ""}`}
        onClick={(e) => {
          e.stopPropagation();

          if (isLocked) {
            return;
          }

          setIsReplaceOpen(false);
          toggleEditor(editorId);
        }}
      >
        <div className="flex items-center gap-2 ">
          {item.isSourceDeleted && (
            <ExclamationTriangle
              size={20}
              className="danger-color-darker shrink-0 mt-px"
              title="This bullet has been deleted. Removing this bullet is permanent."
            />
          )}
          {item.value}
        </div>
      </button>

      {isEditing && (
        <div className={`${styles.editControls} ${styles.editControlsBullet}`}>
          <ButtonContainer
            hasStyle={true}
            isRelative={true}
            align="space-between"
          >
            <Button
              label="Delete"
              styleType="filter"
              labelSlideOut={true}
              iconComponent={() => (
                <Trash3 size={20} className="danger-color" />
              )}
              onClick={() => {
                onBulletRemove();
                closeEditor();
              }}
            />

            <Button
              label="Replace"
              iconComponent={() => <ArrowLeftRight size={20} />}
              styleType="filter"
              labelSlideOut={true}
              onClick={() => setIsReplaceOpen((prev) => !prev)}
            />

            {hasAlternative && (
              <Button
                label="Use Alternative"
                iconComponent={() => <ArrowRepeat size={20} />}
                onClick={() => {}}
                styleType="filter"
                labelSlideOut={true}
              />
            )}

            {!isFirstBullet && (
              <Button
                label="Move Up"
                iconComponent={() => <ArrowUp size={20} />}
                onClick={onBulletMoveUp}
                styleType="filter"
                labelSlideOut={true}
              />
            )}

            {!isLastBullet && (
              <Button
                label="Move Down"
                iconComponent={() => <ArrowDown size={20} />}
                onClick={onBulletMoveDown}
                styleType="filter"
                labelSlideOut={true}
              />
            )}
          </ButtonContainer>

          {isReplaceOpen && (
            <div className={styles.editControlsDropdown} data-resume-dropdown>
              <SearchDropdown
                actions={additionalBullets.map((bullet) => ({
                  label: bullet.value,
                  onClick: () => {
                    onBulletReplace(bullet.libraryBulletId);
                    setIsReplaceOpen(false);
                  },
                }))}
              />
            </div>
          )}
        </div>
      )}
    </li>
  );
};

export default EditableBullet;
