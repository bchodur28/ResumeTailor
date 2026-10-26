import React, { useState } from "react";
import { DropdownContext } from "../../../contexts/DropdownContext";

type ButtonContainerProps = {
  children: React.ReactNode;
  hasStyle?: boolean;
  align?: "space-between" | "center";
  isRelative?: boolean;
};

const getAlignmentClass = (align?: "space-between" | "center") => {
  switch (align) {
    case "space-between":
      return "justify-between";
    case "center":
      return "justify-center";
    default:
      return "justify-start";
  }
};

const ButtonContainer = ({
  children,
  hasStyle,
  align,
  isRelative,
}: ButtonContainerProps) => {
  const [openId, setOpenId] = useState<string | null>(null);

  const toggle = (id: string) => {
    setOpenId((prevOpenId) => (prevOpenId === id ? null : id));
  };
  return (
    <DropdownContext.Provider value={{ openId, toggle }}>
      <div
        className={`btn-container ${hasStyle ? "btn-container-style" : ""} ${getAlignmentClass(align)} ${isRelative ? "btn-container-relative" : "btn-container-absolute"}`}
      >
        {children}
      </div>
    </DropdownContext.Provider>
  );
};

export default ButtonContainer;
