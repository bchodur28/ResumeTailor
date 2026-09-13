import React from "react";

const Card = ({
  children,
  className,
}: {
  children: React.ReactNode;
  className?: string;
}) => {
  return (
    <div className={`box-shadow p-4 rounded-2xl bg-white ${className ?? ""}`}>
      {children}
    </div>
  );
};

export default Card;
