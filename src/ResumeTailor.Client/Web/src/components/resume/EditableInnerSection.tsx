import { useState, useEffect } from "react";
import styles from "./Resume.module.css";
import { useResumeEdit } from "../../contexts/ResumeEditContext";
import ButtonContainer from "../ui/buttons/ButtonContainer";
import SearchableDropdownActions from "../ui/dropdowns/SearchableDropdownActions";
import type { Icon } from "react-bootstrap-icons";
import Button from "../ui/buttons/Button";

export type EditableInnerSectionAction = {
  label: string;
  icon: Icon;
  onClick?: () => void;
  releaseLockOnClick?: boolean;
  additionalActions?: {
    label: string;
    onClick: () => void;
  }[];
};

type EditableInnerSectionProps = {
  editorId: string;
  topPtSpacing: number;
  children: React.ReactNode;
  actions: EditableInnerSectionAction[];
};

const EditableInnerSection = ({
  editorId,
  topPtSpacing,
  children,
  actions,
}: EditableInnerSectionProps) => {
  const { activeEditorId, toggleEditor, closeEditor } = useResumeEdit();

  const [openDropdown, setOpenDropdown] = useState<string | null>(null);

  const isEditing = activeEditorId === editorId;
  const isLocked = activeEditorId !== null && !isEditing;

  const activeDropdownAction = actions.find(
    (action) => action.label === openDropdown,
  );

  useEffect(() => {
    if (!isEditing) {
      setOpenDropdown(null);
    }
  }, [isEditing]);

  return (
    <div
      data-resume-editor-active={isEditing}
      className={`relative ${styles.editableInnerSectionContainer} ${
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

          if (isLocked) return;

          setOpenDropdown(null);
          toggleEditor(editorId);
        }}
      >
        {children}
      </div>

      {isEditing && actions.length > 0 && (
        <div className={styles.editControls}>
          <ButtonContainer
            hasStyle={true}
            isRelative={true}
            align="space-between"
          >
            {actions.map((action) => (
              <Button
                key={action.label}
                label={action.label}
                iconComponent={action.icon}
                styleType="filter"
                labelSlideOut={true}
                onClick={() => {
                  if (action.additionalActions) {
                    setOpenDropdown((prev) =>
                      prev === action.label ? null : action.label,
                    );
                  } else {
                    action.onClick?.();

                    if (action.releaseLockOnClick) {
                      closeEditor();
                    }

                    setOpenDropdown(null);
                  }
                }}
              />
            ))}
          </ButtonContainer>

          {activeDropdownAction?.additionalActions && (
            <div className={styles.editControlsDropdown} data-resume-dropdown>
              <SearchableDropdownActions
                actions={activeDropdownAction.additionalActions.map(
                  (action) => ({
                    label: action.label,
                    onClick: () => {
                      action.onClick();
                      setOpenDropdown(null);
                    },
                  }),
                )}
              />
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default EditableInnerSection;
