import { useState } from "react";
import EditToolBarButton, {
  type EditToolBarButtonProps,
} from "./EditToolBarButton";
import styles from "./Resume.module.css";
import EditToolBar from "./EditToolBar";
import { useResumeEdit } from "../../contexts/ResumeEditContext";

type EditableInnerSectionProps = {
  editorId: string;
  topPtSpacing: number;
  children: React.ReactNode;
  actions: EditToolBarButtonProps[];
};

const EditableInnerSection = ({
  editorId,
  topPtSpacing,
  children,
  actions,
}: EditableInnerSectionProps) => {
  const { activeEditorId, toggleEditor } = useResumeEdit();
  const isEditing = activeEditorId === editorId;

  const isLocked = activeEditorId !== null && activeEditorId !== editorId;

  return (
    <div
      className={`${styles.editableInnerSectionContainer} ${
        isEditing ? styles.editableInnerSectionContainerActive : ""
      }`}
      style={{ paddingTop: `${topPtSpacing}pt` }}
    >
      <div
        className={`${styles.editableInnerSection} ${
          isEditing ? styles.editableInnerSectionAction : ""
        } ${isLocked ? styles.editableControlLocked : ""}`}
        onClick={(e) => {
          e.stopPropagation();

          if (isLocked) {
            return;
          }

          toggleEditor(editorId);
        }}
      >
        {children}
      </div>

      {isEditing && actions.length > 0 && (
        <EditToolBar>
          {actions.map(
            ({ onClick, label, icon, labelClassName, additionalActions }) => (
              <EditToolBarButton
                key={label}
                label={label}
                onClick={onClick}
                icon={icon}
                labelClassName={labelClassName}
                additionalActions={additionalActions}
              />
            ),
          )}
        </EditToolBar>
      )}
    </div>
  );
};

export default EditableInnerSection;
