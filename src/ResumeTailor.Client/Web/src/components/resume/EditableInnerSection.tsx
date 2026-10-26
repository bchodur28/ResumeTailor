import { useState } from "react";
import styles from "./Resume.module.css";
import EditToolBar from "./EditToolBar";
import { useResumeEdit } from "../../contexts/ResumeEditContext";
import ButtonContainer from "../ui/buttons/ButtonContainer";
import SearchDropdown from "../../pages/ResumeDetails/components/SearchDropdown";
import type { Icon } from "react-bootstrap-icons";
import type { EditToolBarButtonProps } from "./EditToolBarButton";
import DropdownButton from "../ui/buttons/DropdownButton";
import Button from "../ui/buttons/Button";

export type EditableInnerSectionAction = {
  label: string;
  icon: Icon;
  onClick?: () => void;
  additionalActions?: { label: string; onClick: () => void }[];
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
        <ButtonContainer
          hasStyle={true}
          isRelative={false}
          align="space-between"
        >
          {actions.map(({ onClick, label, icon, additionalActions }) =>
            onClick ? (
              <Button
                key={label}
                label={label}
                iconComponent={icon}
                styleType="filter"
                onClick={() => onClick?.()}
                labelSlideOut={true}
              />
            ) : (
              <DropdownButton
                key={label}
                label={label}
                iconComponent={icon}
                styleType="filter"
                labelSlideOut={true}
                positionRelativeTo="container"
              >
                <SearchDropdown
                  filteredActions={
                    additionalActions?.map(({ label, onClick }) => ({
                      label,
                      onClick,
                    })) ?? []
                  }
                />
              </DropdownButton>
            ),
          )}
        </ButtonContainer>
      )}
    </div>
  );
};

export default EditableInnerSection;
