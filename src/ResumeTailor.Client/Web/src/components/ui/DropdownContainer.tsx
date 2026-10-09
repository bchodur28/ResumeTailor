export type DropdownContainerProps = {
  children: React.ReactNode;
  alignment?: "left" | "right" | "center";
};

const DropdownContainer = ({
  children,
  alignment = "left",
}: DropdownContainerProps) => {
  return (
    <div
      className={`absolute top-full z-10 mt-2 min-w-44 overflow-hidden rounded-lg border border-gray-300 bg-white shadow-lg ${
        alignment === "right"
          ? "right-0"
          : alignment === "center"
            ? "left-1/2 transform -translate-x-1/2"
            : "left-0"
      }`}
    >
      {children}
    </div>
  );
};

export default DropdownContainer;
