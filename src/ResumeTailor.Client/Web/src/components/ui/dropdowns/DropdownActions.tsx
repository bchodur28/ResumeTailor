export type DropdownProps = {
  actions: { label: string; onClick: () => void }[];
};

const DropdownActions = ({ actions }: DropdownProps) => {
  return (
    <>
      <ul className="dropdown-actions">
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

export default DropdownActions;
