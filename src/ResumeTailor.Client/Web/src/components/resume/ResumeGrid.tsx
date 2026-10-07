import React from "react";

const ResumeGrid = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="grid grid-cols-1 gap-4 min-[1200px]:grid-cols-2 min-[1600px]:grid-cols-3 min-[2200px]:grid-cols-4 self-center">
      {children}
    </div>
  );
};

export default ResumeGrid;
