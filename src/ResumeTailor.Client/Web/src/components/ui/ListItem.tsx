import type { ReactNode } from "react";
import ActionDropdown from "./ActionDropdown";

type Item = {
  content: string;
  tags?: string[];
  icon?: ReactNode;
  actions?: {
    actionName: string;
    actionFn: () => void;
    dropDownAction?: boolean;
    btnClassReplace?: string;
  }[];
  borderColor?: string;
};

const ListItem = ({ content, tags, icon, actions, borderColor }: Item) => {
  const primaryActions =
    actions?.filter((action) => !action.dropDownAction) ?? [];

  const dropdownActions =
    actions?.filter((action) => action.dropDownAction) ?? [];

  return (
    <li
      className={`
        border p-2 rounded-xl mt-2
        ${borderColor ?? "border-gray-300"}
        shadow bg-white
      `}
    >
      <div className="flex flex-col gap-3">
        {/* Content section and icon */}
        <div className="flex min-w-0 flex-1 items-center gap-2">
          {icon && <span className="shrink-0">{icon}</span>}

          <p className="min-w-0 [overflow-wrap: anywhere] font-semibold text-gray-800">
            {content.replaceAll("_", " ")}
          </p>
        </div>

        {/* Tags section and button*/}
        <div className="flex justify-between">
          <div className="flex flex-wrap gap-2">
            {tags?.map((tag, index) => (
              <div
                key={index}
                className="min-w-0 max-w-full rounded-2xl border border-gray-300 bg-gray-100 px-3 py-2 shadow"
              >
                <p className="wrap-break-word text-sm font-semibold text-gray-600">
                  {tag}
                </p>
              </div>
            ))}
          </div>

          <div className="flex shrink-0 gap-2 self-end">
            {primaryActions.map((action, index) => (
              <button
                key={index}
                className={action.btnClassReplace ?? "btn-secondary"}
                onClick={action.actionFn}
              >
                {action.actionName}
              </button>
            ))}

            {dropdownActions.length > 0 && (
              <ActionDropdown
                actions={dropdownActions.map((action) => ({
                  actionName: action.actionName,
                  actionFn: action.actionFn,
                }))}
              />
            )}
          </div>
        </div>
      </div>
    </li>
  );
};

export default ListItem;
