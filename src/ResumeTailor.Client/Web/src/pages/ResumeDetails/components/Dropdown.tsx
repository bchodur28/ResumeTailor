import styles from "../../../components/resume/Resume.module.css";

export type DropdownProps = {
  actions: { label: string; onClick: () => void }[];
};

const Dropdown = ({ actions }: DropdownProps) => {
  return (
    <>
      <ul className={styles.editToolBarList}>
        {actions.map((action) => (
          <li key={action.label}>
            <button
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                action.onClick();
              }}
            >
              {action.label}
            </button>
          </li>
        ))}
      </ul>
    </>
  );
};

export default Dropdown;
