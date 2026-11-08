import type { Icon } from "react-bootstrap-icons";

export type ButtonProps = {
  label: string;
  onClick?: () => void;
  iconComponent?: Icon;
  type?: "submit";
  styleType?: "plain" | "confirm" | "danger" | "filter" | "edit";
  hideLabel?: boolean;
  isActive?: boolean;
  labelSlideOut?: boolean;
  optionalWarning?: string;
};

const getStyleClass = (styleType: string | undefined) => {
  switch (styleType) {
    case "plain":
      return { normal: "btn--secondary", active: "" };
    case "confirm":
      return { normal: "btn--confirm", active: "" };
    case "danger":
      return { normal: "btn--danger", active: "" };
    case "filter":
      return { normal: "btn--filter", active: "btn--filter--active" };
    case "edit":
      return { normal: "btn--edit", active: "btn--edit--active" };
    default:
      return { normal: "btn--main", active: "btn--main--active" };
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
  optionalWarning,
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
      <div className="flex flex-col items-center">
        <div className="btn-content-container">
          {Icon && <div className="btn-icon">{<Icon size={20} />}</div>}
          <p
            className={`${labelSlideOut ? "btn-label-slide-out" : "btn-label"} ${Icon && (hideLabel ?? true) ? "btn-content-hidden" : ""}`}
          >
            {!hideLabel && label}
          </p>
        </div>

        {optionalWarning && (
          <p className="btn__optional-warning">{optionalWarning}</p>
        )}
      </div>
    </button>
  );
};

export default Button;
