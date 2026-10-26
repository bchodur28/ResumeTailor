import { useState } from "react";
import {
  ArrowUp,
  ArrowDown,
  ArrowRepeat,
  Trash3,
  ArrowLeftRight,
  ExclamationTriangle,
} from "react-bootstrap-icons";

import type { ResumeBulletViewModel } from "../../models/forms/ResumeViewModel";
import type { BulletResponse } from "../../api/contracts/bullets/BulletResponse";

import styles from "./Resume.module.css";
import EditToolBar from "./EditToolBar";
import EditToolBarButton from "./EditToolBarButton";
import { useResumeEdit } from "../../contexts/ResumeEditContext";
import ButtonContainer from "../ui/buttons/ButtonContainer";
import Button from "../ui/buttons/Button";
import DropdownButton from "../ui/buttons/DropdownButton";
import SearchDropdown from "../../pages/ResumeDetails/components/SearchDropdown";

type EditableBulletProps = {
  item: ResumeBulletViewModel;
  additionalBullets: BulletResponse[];
  companyIndex: number;
  bulletIndex: number;
  isLastBullet: boolean;
  isFirstBullet: boolean;

  onBulletChange: (
    companyIndex: number,
    bulletIndex: number,
    bulletId: number,
    value: string,
  ) => void;

  onBulletMoveUp: () => void;
  onBulletMoveDown: () => void;
};

const EditableBullet = ({
  item,
  additionalBullets,
  companyIndex,
  bulletIndex,
  isLastBullet,
  isFirstBullet,
  onBulletChange,
  onBulletMoveUp,
  onBulletMoveDown,
}: EditableBulletProps) => {
  const { activeEditorId, toggleEditor, closeEditor } = useResumeEdit();

  const bulletId = item.id ?? item.sourceBulletId ?? item.id ?? bulletIndex;

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
        <ButtonContainer
          hasStyle={true}
          isRelative={false}
          align="space-between"
        >
          <Button
            label="Delete"
            styleType="filter"
            labelSlideOut={true}
            iconComponent={() => <Trash3 size={20} className="danger-color" />}
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

          <DropdownButton
            label="Replace"
            iconComponent={() => <ArrowLeftRight size={20} />}
            styleType="filter"
            labelSlideOut={true}
            positionRelativeTo="container"
          >
            <SearchDropdown
              filteredActions={additionalBullets.map((bullet) => ({
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
            ></SearchDropdown>
          </DropdownButton>

          {hasAlternative && (
            <Button
              label="Use Alternative"
              iconComponent={() => <ArrowRepeat size={20} />}
              onClick={() =>
                handleBulletSwapWithAlternative(item.alternativeValue!)
              }
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
      )}
    </li>
  );
};

export default EditableBullet;
