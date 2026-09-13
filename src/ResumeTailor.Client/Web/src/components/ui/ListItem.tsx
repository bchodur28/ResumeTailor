import type { ReactNode } from "react";

type ListItemProps = {
  value: string;
  borderColor?: string;
  leftContent?: ReactNode;
};

const ListItem = ({ value, borderColor, leftContent }: ListItemProps) => {
  return (
    <li
      className={`flex items-center gap-2 border p-2 rounded-xl mt-2 ${borderColor ?? "border-gray-300"} shadow`}
    >
      {leftContent && <span>{leftContent}</span>}
      {value}
    </li>
  );
};

export default ListItem;
