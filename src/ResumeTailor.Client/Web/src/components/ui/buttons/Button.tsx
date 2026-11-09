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
  hideLabelOnMobile?: boolean;
  btnClassName?: string;
  grow?: boolean;
};

const getStyleClass = (styleType: string | undefined) => {
  switch (styleType) {
    case "plain":
      return { normal: "btn--plain", active: "" };
    case "confirm":
      return { normal: "btn--confirm", active: "" };
    case "danger":
      return { normal: "btn--danger", active: "" };
    case "filter":
      return { normal: "btn--filter", active: "btn--active" };
    case "edit":
      return { normal: "btn--edit", active: "btn--active" };
    default:
      return { normal: "btn--main", active: "btn--active" };
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
  hideLabelOnMobile = false,
  optionalWarning,
  grow,
}: ButtonProps) => {
  return (
    <button
      onClick={(e) => {
        e.stopPropagation();
        onClick?.();
      }}
      type={type ?? "button"}
      className={`
        btn
        ${grow ? "flex-1 min-w-0" : "w-fit"}
        ${getStyleClass(styleType).normal}
        ${isActive ? getStyleClass(styleType).active : ""}
      `}
      aria-label={label}
    >
      <div className="flex flex-col items-center">
        <div className="btn__content">
          {Icon && <div className="btn__icon">{<Icon size={20} />}</div>}
          <p
            className={`
              ${labelSlideOut ? "btn__label--slide-out" : "btn__label"} 
              ${Icon && (hideLabel ?? true) && !labelSlideOut ? "btn__label--hidden" : ""}
              ${hideLabelOnMobile ? "btn__label--mobile-hidden" : ""}`}
          >
            {label}
          </p>
        </div>

        {optionalWarning && (
          <p className="btn__optional--warning">{optionalWarning}</p>
        )}
      </div>
    </button>
  );
};

export default Button;
