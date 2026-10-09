import type { Icon } from "react-bootstrap-icons";

export type ButtonProps = {
  label: string;
  onClick: () => void;
  iconComponent?: Icon;
  type?: "submit";
  styleType?: "plain" | "confirm" | "danger" | "filter";
  hideLabel?: boolean;
};

const getStyleClass = (styleType: string | undefined) => {
  switch (styleType) {
    case "plain":
      return "btn-secondary";
    case "confirm":
      return "confirm-btn";
    case "danger":
      return "remove-btn";
    case "filter":
      return "filter-btn";
    default:
      return "btn";
  }
};

const Button = ({
  label,
  onClick,
  iconComponent: Icon,
  type,
  styleType,
  hideLabel = false,
}: ButtonProps) => {
  return (
    <button
      onClick={onClick}
      type={type ?? "button"}
      className={`flex gap-2 items-center  ${getStyleClass(styleType)} `}
      aria-label={label}
    >
      {Icon && <Icon size={20} />}
      {!hideLabel && label}
    </button>
  );
};

export default Button;
