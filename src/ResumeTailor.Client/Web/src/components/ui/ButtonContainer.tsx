import React from "react";

const ButtonContainer = ({ children }: { children: React.ReactNode }) => {
  return <div className="btn-container">{children}</div>;
};

export default ButtonContainer;
