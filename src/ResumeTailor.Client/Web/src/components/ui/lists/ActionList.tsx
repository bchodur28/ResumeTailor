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

  const className = `p-2 ${fontColor} ${bgColor} text-sm ${bold}`;

  return className;
};

const ActionList = ({ actions }: ActionListProps) => {
  return (
    <ul className="flex flex-col justify-end">
      {actions.map(({ actionName, actionFn, actionIcon: Icon }, index) => (
        <button className={createClassNames()} key={index} onClick={actionFn}>
          {Icon && <Icon size={16} />}
          {actionName}
        </button>
      ))}
    </ul>
  );
};

export default ActionList;
