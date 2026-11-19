import React, { useRef, useState, useEffect } from "react";
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

  const containerRef = useRef<HTMLDivElement | null>(null);

  const toggle = (id: string) => {
    setOpenId((prevOpenId) => (prevOpenId === id ? null : id));
  };

  const close = () => {
    setOpenId(null);
  };

  useEffect(() => {
    if (openId === null) return;

    const handleClickOutside = (event: PointerEvent) => {
      if (
        event.target instanceof Node &&
        !containerRef.current?.contains(event.target)
      ) {
        close();
      }
    };

    document.addEventListener("pointerdown", handleClickOutside);

    return () => {
      document.removeEventListener("pointerdown", handleClickOutside);
    };
  }, [openId]);

  return (
    <DropdownContext.Provider value={{ openId, toggle, close }}>
      <div
        ref={containerRef}
        className={`btn-container ${hasStyle ? "btn-container--styled" : ""} ${getAlignmentClass(align)} ${isRelative ? "btn-container--relative" : "btn-container--absolute"}`}
      >
        {children}
      </div>
    </DropdownContext.Provider>
  );
};

export default ButtonContainer;
