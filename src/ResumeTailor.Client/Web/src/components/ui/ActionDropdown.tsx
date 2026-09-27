import { useEffect, useRef, useState } from "react";

export type ActionDropdownAction = {
  actionName: string;
  actionFn: () => void;
};

type ActionDropdownProps = {
  actions: ActionDropdownAction[];
  label?: string;
  onClick?: () => void;
};

const ActionDropdown = ({
  actions,
  label = "Actions",
  onClick,
}: ActionDropdownProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const isDisabled = actions.length === 0 && !onClick;

  useEffect(() => {
    if (!isOpen) return;

    const handlePointerDown = (event: PointerEvent) => {
      if (
        event.target instanceof Node &&
        !dropdownRef.current?.contains(event.target)
      ) {
        setIsOpen(false);
      }
    };
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") setIsOpen(false);
    };

    document.addEventListener("pointerdown", handlePointerDown);
    document.addEventListener("keydown", handleKeyDown);
    return () => {
      document.removeEventListener("pointerdown", handlePointerDown);
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [isOpen]);

  const toggleMenu = () => {
    if (actions.length > 0) setIsOpen((open) => !open);
  };

  const handlePrimaryClick = () => {
    if (onClick) {
      setIsOpen(false);
      onClick();
      return;
    }
    toggleMenu();
  };

  return (
    <div className="action-dropdown" ref={dropdownRef}>
      <div className="action-dropdown__button-group">
        <button
          type="button"
          className="action-dropdown__primary"
          disabled={isDisabled}
          onClick={handlePrimaryClick}
        >
          {label}
        </button>
        <button
          type="button"
          className="action-dropdown__toggle"
          aria-label={`${label} options`}
          aria-haspopup="menu"
          aria-expanded={isOpen}
          disabled={actions.length === 0}
          onClick={toggleMenu}
        >
          <svg
            aria-hidden="true"
            viewBox="0 0 16 16"
            width="14"
            height="14"
            fill="currentColor"
          >
            <path d="M2.5 5.5 8 11l5.5-5.5-1.4-1.4L8 8.2 3.9 4.1z" />
          </svg>
        </button>
      </div>

      {isOpen && (
        <div
          className="absolute right-0 top-full z-10 mt-2 min-w-44 overflow-hidden rounded-lg border border-gray-200 bg-white py-1 shadow-lg"
          role="menu"
        >
          {actions.map((action, index) => (
            <button
              key={`${action.actionName}-${index}`}
              type="button"
              className="w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
              role="menuitem"
              onClick={() => {
                setIsOpen(false);
                action.actionFn();
              }}
            >
              {action.actionName}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default ActionDropdown;
