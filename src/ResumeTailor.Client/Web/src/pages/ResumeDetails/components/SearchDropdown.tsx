import { useState } from "react";
import styles from "../../../components/resume/Resume.module.css";

export type SearchDropdownProps = {
  filteredActions: { label: string; onClick: () => void }[];
};

const SearchDropdown = ({ filteredActions }: SearchDropdownProps) => {
  const [searchValue, setSearchValue] = useState("");

  return (
    <div>
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
  );
};

export default SearchDropdown;
