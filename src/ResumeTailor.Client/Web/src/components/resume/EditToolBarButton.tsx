import type { Icon } from "react-bootstrap-icons";
import { useState } from "react";
import styles from "./Resume.module.css";

export type EditToolBarAction = {
  label: string;
  onClick: () => void;
};

export type EditToolBarButtonProps = {
  label: string;
  icon: Icon;
  onClick?: () => void;
  additionalActions?: EditToolBarAction[];
  labelClassName?: string;
};

const EditToolBarButton = ({
  label,
  icon: Icon,
  onClick,
  additionalActions,
  labelClassName,
}: EditToolBarButtonProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [searchValue, setSearchValue] = useState("");

  const hasAdditionalActions =
    additionalActions != null && additionalActions.length > 0;

  const filteredActions =
    additionalActions?.filter((action) =>
      action.label.toLowerCase().includes(searchValue.toLowerCase()),
    ) ?? [];

  const handleClick = () => {
    if (hasAdditionalActions) {
      setIsOpen(!isOpen);
      return;
    }

    onClick?.();
  };
  return (
    <div
      className={`${styles.editToolBarButtonContainer} ${
        isOpen ? styles.editToolBarButtonContainerOpen : ""
      }`}
    >
      <button
        type="button"
        className={styles.editToolBarButton}
        onClick={(e) => {
          e.stopPropagation();
          handleClick();
        }}
      >
        <Icon />
        <span className={labelClassName}>{label}</span>
      </button>

      {isOpen && hasAdditionalActions && (
        <div className={styles.editToolBarDropdown}>
          <div className={styles.editToolBarSearchContainer}>
            <input
              type="text"
              value={searchValue}
              placeholder="Search..."
              className={styles.editToolBarSearch}
              onClick={(e) => e.stopPropagation()}
              onChange={(e) => setSearchValue(e.target.value)}
            />
          </div>
          <ul className={styles.editToolBarList}>
            {filteredActions.map((action) => (
              <li key={action.label}>
                <button
                  type="button"
                  onClick={(e) => {
                    e.stopPropagation();
                    action.onClick();

                    setIsOpen(false);
                  }}
                >
                  {action.label}
                </button>
              </li>
            ))}
            {filteredActions.length === 0 && (
              <li className={styles.editToolBarNoResults}>No results found</li>
            )}
          </ul>
        </div>
      )}
    </div>
  );
};

export default EditToolBarButton;
