import type { Icon } from "react-bootstrap-icons";

export type ButtonProps = {
  label: string;
  onClick?: () => void;
  iconComponent?: Icon;
  type?: "submit";
  styleType?: "plain" | "confirm" | "danger" | "filter";
  hideLabel?: boolean;
  isActive?: boolean;
  labelSlideOut?: boolean;
};

const getStyleClass = (styleType: string | undefined) => {
  switch (styleType) {
    case "plain":
      return { normal: "btn-secondary", active: "" };
    case "confirm":
      return { normal: "confirm-btn", active: "" };
    case "danger":
      return { normal: "remove-btn", active: "" };
    case "filter":
      return { normal: "btn-filter", active: "btn-filter-active" };
    default:
      return { normal: "btn-main", active: "btn-main-active" };
  }
};

const Button = ({
  label,
  onClick,
  iconComponent: Icon,
  type,
  styleType,
  hideLabel,
  isActive = false,
  labelSlideOut = false,
}: ButtonProps) => {
  return (
    <button
      onClick={(e) => {
        e.stopPropagation();
        onClick?.();
      }}
      type={type ?? "button"}
      className={`btn-base ${getStyleClass(styleType).normal} ${isActive ? `${getStyleClass(styleType).active}` : ""}`}
      aria-label={label}
    >
      <div className="btn-content-container">
        <div className="btn-icon">{Icon && <Icon size={20} />}</div>
        <p
          className={`${labelSlideOut ? "btn-label-slide-out" : "btn-label"} ${Icon && (hideLabel ?? true) ? "btn-content-hidden" : ""}`}
        >
          {!hideLabel && label}
        </p>
      </div>
    </button>
  );
};

export default Button;
