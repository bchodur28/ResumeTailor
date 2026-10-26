import Button from "./Button";
import { useId, useState } from "react";
import { useDropdownContext } from "../../../contexts/DropdownContext";
import type { ButtonProps } from "./Button";
import DropdownContainer from "../dropdowns/DropdownContainer";

export type DropdownButtonProps = Omit<ButtonProps, "onClick"> & {
  children?: React.ReactNode;
  positionRelativeTo?: "button" | "container";
};

const DropdownButton = ({
  label,
  iconComponent,
  children,
  styleType,
  hideLabel,
  labelSlideOut,
  positionRelativeTo = "button",
}: DropdownButtonProps) => {
  const id = useId();
  const context = useDropdownContext();

  const [localOpen, setLocalOpen] = useState(false);

  const isOpen = context ? context.openId === id : localOpen;

  const toggleDropdown = () => {
    if (context) {
      context.toggle(id);
    } else {
      setLocalOpen((prev) => !prev);
    }
  };
  return (
    <div
      className={`${
        positionRelativeTo === "container"
          ? "dropdown-btn-container-parent-width"
          : "dropdown-btn-container"
      }`}
    >
      <Button
        label={label}
        onClick={toggleDropdown}
        iconComponent={iconComponent}
        styleType={styleType}
        hideLabel={hideLabel}
        labelSlideOut={labelSlideOut}
        isActive={isOpen}
      />
      {isOpen && (
        <DropdownContainer
          matchParentWidth={positionRelativeTo === "container"}
        >
          {children}
        </DropdownContainer>
      )}
    </div>
  );
};

export default DropdownButton;
