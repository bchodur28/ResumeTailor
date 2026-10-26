export type DropdownContainerProps = {
  children: React.ReactNode;
  matchParentWidth?: boolean;
};

const DropdownContainer = ({
  children,
  matchParentWidth = false,
}: DropdownContainerProps) => {
  return (
    <div
      className={`dropdown-container ${matchParentWidth ? "dropdown-container-parent-width" : ""}`}
    >
      {children}
    </div>
  );
};

export default DropdownContainer;
