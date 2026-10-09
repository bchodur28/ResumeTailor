import Button from "./Button";
import { useState } from "react";
import type { ButtonProps } from "./Button";
import DropdownContainer from "./DropdownContainer";

export type DropdownButtonProps = Omit<ButtonProps, "onClick"> & {
  children?: React.ReactNode;
  containerAlignment?: "left" | "right" | "center";
};

const DropdownButton = ({
  label,
  iconComponent,
  children,
  styleType,
  hideLabel,
  containerAlignment,
}: DropdownButtonProps) => {
  const [isOpen, setIsOpen] = useState(false);
  return (
    <div className="inline-block relative">
      <Button
        label={label}
        onClick={() => setIsOpen(!isOpen)}
        iconComponent={iconComponent}
        styleType={styleType}
        hideLabel={hideLabel}
      />
      {isOpen && (
        <DropdownContainer alignment={containerAlignment}>
          {children}
        </DropdownContainer>
      )}
    </div>
  );
};

export default DropdownButton;
