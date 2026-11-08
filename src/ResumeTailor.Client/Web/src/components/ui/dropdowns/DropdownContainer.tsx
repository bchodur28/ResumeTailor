export type DropdownContainerProps = {
  children: React.ReactNode;
  matchParentWidth?: boolean;
  closeOnClick?: boolean;
  onClose?: () => void;
};

const DropdownContainer = ({
  children,
  matchParentWidth = false,
  closeOnClick = false,
  onClose,
}: DropdownContainerProps) => {
  const handleClick = () => {
    if (closeOnClick) {
      onClose?.();
    }
  };
  return (
    <div
      onClick={handleClick}
      className={
        matchParentWidth
          ? "dropdown-container-parent-width"
          : "dropdown-container"
      }
    >
      {children}
    </div>
  );
};

export default DropdownContainer;
