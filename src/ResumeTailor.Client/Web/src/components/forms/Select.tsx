import type { Option } from "../../models/forms/Option";
import { useEffect, useRef, useState } from "react";

type SelectProps = {
  id: string;
  label: string;
  options: Option[];
  onChange: (option: Option | undefined) => void;
  onBlur?: () => void;
  value?: Option;
  isRequired?: boolean;
  error?: string;
  placeholder?: string;
};

const Select = ({
  id,
  label,
  options,
  onChange,
  onBlur,
  value,
  isRequired,
  error,
  placeholder = "Select an option",
}: SelectProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  const [isFocused, setIsFocused] = useState(false);

  const optionRefs = useRef<(HTMLLIElement | null)[]>([]);

  const visibleError = isFocused ? undefined : error;

  useEffect(() => {
    if (isOpen && highlightedIndex >= 0) {
      optionRefs.current[highlightedIndex]?.scrollIntoView({
        block: "nearest",
      });
    }
  }, [highlightedIndex, isOpen]);

  const openDropdown = () => {
    if (options.length === 0) return;

    const selectedIndex = options.findIndex(
      (option) => option.id === value?.id,
    );

    setHighlightedIndex(selectedIndex >= 0 ? selectedIndex : 0);
    setIsOpen(true);
  };

  const handleSelect = (option: Option) => {
    onChange(option);
    setIsOpen(false);
    setHighlightedIndex(-1);
  };

  const handleBlur = () => {
    setIsFocused(false);
    setIsOpen(false);
    setHighlightedIndex(-1);
    onBlur?.();
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLButtonElement>) => {
    if (e.key === "ArrowDown" || e.key === "ArrowUp") {
      e.preventDefault();

      if (!isOpen) {
        openDropdown();
        return;
      }

      setHighlightedIndex((current) => {
        if (e.key === "ArrowDown") {
          return current < options.length - 1 ? current + 1 : 0;
        }

        return current > 0 ? current - 1 : options.length - 1;
      });
    }

    if (e.key === "Enter" || e.key === " ") {
      e.preventDefault();

      if (!isOpen) {
        openDropdown();
        return;
      }

      if (highlightedIndex >= 0 && options[highlightedIndex]) {
        handleSelect(options[highlightedIndex]);
      } else {
        setIsOpen(false);
      }
    }

    if (e.key === "Escape") {
      e.preventDefault();
      setIsOpen(false);
      setHighlightedIndex(-1);
    }
  };

  return (
    <div className="flex flex-col relative">
      <div
        className={`flex ${visibleError ? "justify-between" : "justify-start"}`}
      >
        <label className="text-gray-500 text-sm font-medium" htmlFor={id}>
          {label}
        </label>

        {visibleError && <span className="text-red-500">{visibleError}</span>}
      </div>

      <button
        type="button"
        id={id}
        className="border p-2 rounded border-gray-300 text-left"
        onClick={() => {
          if (isOpen) {
            setIsOpen(false);
          } else {
            openDropdown();
          }
        }}
        onFocus={() => setIsFocused(true)}
        onBlur={handleBlur}
        onKeyDown={handleKeyDown}
        role="combobox"
        aria-haspopup="listbox"
        aria-expanded={isOpen}
        aria-controls={`${id}-listbox`}
        aria-activedescendant={
          isOpen && highlightedIndex >= 0
            ? `${id}-option-${highlightedIndex}`
            : undefined
        }
        aria-required={isRequired}
        aria-invalid={!!error}
      >
        <span className={value ? "" : "text-gray-400"}>
          {value?.label ?? placeholder}
        </span>
      </button>

      {isOpen && options.length > 0 && (
        <ul
          id={`${id}-listbox`}
          role="listbox"
          aria-label={label}
          className="absolute top-full left-0 right-0 z-10 border border-gray-300 rounded mt-1 max-h-60 overflow-y-auto bg-white"
        >
          {options.map((option, index) => (
            <li
              id={`${id}-option-${index}`}
              key={option.id}
              role="option"
              aria-selected={option.id === value?.id}
              ref={(element) => {
                optionRefs.current[index] = element;
              }}
              className={`p-2 cursor-pointer ${
                index === highlightedIndex ? "bg-gray-200" : "hover:bg-gray-200"
              }`}
              onMouseEnter={() => setHighlightedIndex(index)}
              onMouseDown={(e) => e.preventDefault()}
              onClick={() => handleSelect(option)}
            >
              {option.label}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Select;
