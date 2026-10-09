import { useState } from "react";

export type FilterButtonProps = {
  value: string;
};

const FilterButton = ({ value }: FilterButtonProps) => {
  const [isOpen, setIsOpen] = useState(false);
  return (
    <div className="border inline-block relative">
      <button
        className="filter-btn"
        onClick={() => {
          setIsOpen(!isOpen);
        }}
      >
        {value}
      </button>
      {isOpen && (
        <div className="absolute top-full z-10 mt-2 min-w-44 overflow-hidden rounded-lg border border-gray-300 bg-white py-1 shadow-lg">
          <p>Filter options go here</p>
        </div>
      )}
    </div>
  );
};

export default FilterButton;
