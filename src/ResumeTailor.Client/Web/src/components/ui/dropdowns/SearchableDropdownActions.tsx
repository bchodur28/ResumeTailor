import { useState } from "react";
import DropdownActions from "./DropdownActions";

export type SearchableDropdownActionsProps = {
  actions: { label: string; onClick: () => void }[];
};

const SearchableDropdownActions = ({
  actions,
}: SearchableDropdownActionsProps) => {
  const [searchValue, setSearchValue] = useState("");

  const filteredActions = actions.filter((action) =>
    action.label.toLowerCase().includes(searchValue.trim().toLowerCase()),
  );

  return (
    <div className="w-full min-w-0">
      <div className="dropdown-actions-search">
        <input
          type="text"
          value={searchValue}
          placeholder="Search..."
          className="dropdown-actions-search__input"
          onClick={(e) => e.stopPropagation()}
          onChange={(e) => setSearchValue(e.target.value)}
        />
      </div>

      <DropdownActions actions={filteredActions} />

      {filteredActions.length === 0 && (
        <div className="dropdown-actions-search__no-results">
          No results found
        </div>
      )}
    </div>
  );
};

export default SearchableDropdownActions;
