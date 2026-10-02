import { useState } from "react";
import {
  ArrowUp,
  ArrowDown,
  ArrowRepeat,
  Trash3,
  ArrowLeftRight,
  ExclamationTriangle,
} from "react-bootstrap-icons";

import type { ResumeBulletResult } from "../../models/resumes/ResumeBulletResult";
import type { BulletResponse } from "../../api/contracts/bullets/BulletResponse";

import styles from "./Resume.module.css";
import EditToolBar from "./EditToolBar";
import EditToolBarButton from "./EditToolBarButton";
import { useResumeEdit } from "../../contexts/ResumeEditContext";

type EditableBulletProps = {
  item: ResumeBulletResult;
  additonalBullets: BulletResponse[];
  companyIndex: number;
  bulletIndex: number;
  isLastBullet: boolean;
  isFirstBullet: boolean;

  onBulletChange: (
    companyIndex: number,
    bulletIndex: number,
    sourceBulletId: number | null,
    value: string,
  ) => void;

  onBulletMoveUp: () => void;
  onBulletMoveDown: () => void;
};

const EditableBullet = ({
  item,
  additonalBullets,
  companyIndex,
  bulletIndex,
  isLastBullet,
  isFirstBullet,
  onBulletChange,
  onBulletMoveUp,
  onBulletMoveDown,
}: EditableBulletProps) => {
  const { activeEditorId, toggleEditor, closeEditor } = useResumeEdit();

  const bulletId =
    item.id ?? item.sourceBulletId ?? item.selectionId ?? bulletIndex;

  const editorId = `bullet:${companyIndex}:${bulletId}`;

  const isEditing = activeEditorId === editorId;

  const isLocked = activeEditorId !== null && activeEditorId !== editorId;

  const hasAlternative = item.alternativeValue !== null;

  const handleBulletSwapWithAlternative = (alternativeValue: string) => {
    onBulletChange(
      companyIndex,
      bulletIndex,
      item.sourceBulletId,
      alternativeValue,
    );
  };

  return (
    <li
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
        <EditToolBar>
          <EditToolBarButton
            label="Delete"
            icon={() => <Trash3 size={20} className="danger-color" />}
            labelClassName="danger-color"
            onClick={() => {
              onBulletChange(
                companyIndex,
                bulletIndex,
                item.sourceBulletId,
                "",
              );
              closeEditor();
            }}
          />

          <EditToolBarButton
            label="Replace"
            icon={() => <ArrowLeftRight size={20} />}
            additionalActions={additonalBullets.map((bullet) => ({
              label: bullet.value,
              onClick: () => {
                onBulletChange(
                  companyIndex,
                  bulletIndex,
                  bullet.id,
                  bullet.value,
                );
              },
            }))}
          />

          {hasAlternative && (
            <EditToolBarButton
              label="Use Alternative"
              icon={() => <ArrowRepeat size={20} />}
              onClick={() =>
                handleBulletSwapWithAlternative(item.alternativeValue!)
              }
            />
          )}

          {!isFirstBullet && (
            <EditToolBarButton
              label="Move Up"
              icon={() => <ArrowUp size={20} />}
              onClick={onBulletMoveUp}
            />
          )}

          {!isLastBullet && (
            <EditToolBarButton
              label="Move Down"
              icon={() => <ArrowDown size={20} />}
              onClick={onBulletMoveDown}
            />
          )}
        </EditToolBar>
      )}
    </li>
  );
};

export default EditableBullet;
