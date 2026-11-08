import type { Icon } from "react-bootstrap-icons";

export type ActionListProps = {
  actions: {
    actionName: string;
    actionFn: () => void;
    actionIcon?: Icon;
    overrideStyles?: {
      isBold: boolean;
      fontColorClass: string;
      bgColorClass: string;
    };
  }[];
  onAction?: () => void;
};

const createClassNames = (overrideStyles?: {
  isBold: boolean;
  fontColorClass: string;
  bgColorClass: string;
}) => {
  const bold = overrideStyles?.isBold ? "" : "font-semibold";
  const fontColor =
    overrideStyles && overrideStyles.fontColorClass.trim() !== ""
      ? overrideStyles.fontColorClass
      : "text-gray-700";

  const bgColor =
    overrideStyles && overrideStyles.bgColorClass.trim() !== ""
      ? overrideStyles.bgColorClass
      : "bg-white hover:bg-gray-100";

  const className = `p-3 ${fontColor} ${bgColor} text-sm ${bold}`;

  return className;
};

const ActionList = ({ actions, onAction }: ActionListProps) => {
  return (
    <ul>
      {actions.map(
        ({ actionName, actionFn, actionIcon: Icon, overrideStyles }, index) => (
          <li className="flex flex-col" key={index}>
            <button
              type="button"
              className={createClassNames(overrideStyles)}
              onClick={() => {
                actionFn();
                onAction?.();
              }}
            >
              {Icon && <Icon size={16} />}
              {actionName}
            </button>
          </li>
        ),
      )}
    </ul>
  );
};

export default ActionList;
