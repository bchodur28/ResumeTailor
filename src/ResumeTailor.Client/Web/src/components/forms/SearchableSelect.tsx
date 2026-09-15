import type { Option } from "../../models/forms/Option";
import { useEffect, useState, useRef } from "react";

type SearchableSelectProps = {
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

const SearchableSelect = ({
  id,
  label,
  options,
  onChange,
  onBlur,
  value,
  isRequired,
  error,
  placeholder,
}: SearchableSelectProps) => {
  const [search, setSearch] = useState("");
  const [isOpen, setIsOpen] = useState(false);
  const [highlightedIndex, setHighlightedIndex] = useState(-1);
  const [isFocused, setIsFocused] = useState(false);

  const visibleError = isFocused ? undefined : error;

  const optionRefs = useRef<(HTMLLIElement | null)[]>([]);

  useEffect(() => {
    setSearch(value?.label ?? "");
  }, [value]);

  useEffect(() => {
    if (highlightedIndex >= 0 && optionRefs.current[highlightedIndex]) {
      optionRefs.current[highlightedIndex]?.scrollIntoView({
        block: "nearest",
      });
    }
  }, [highlightedIndex]);

  const filteredOptions = options.filter((option) =>
    option.label.toLowerCase().includes(search.toLowerCase()),
  );

  const findExactMatch = (text: string) =>
    options.find(
      (option) => option.label.toLowerCase() === text.trim().toLowerCase(),
    );

  const handleSelect = (option: Option) => {
    setSearch(option.label);
    setIsOpen(false);
    setHighlightedIndex(-1);

    onChange(option);
  };

  const handleBlur = () => {
    setIsFocused(false);
    setIsOpen(false);

    if (search.trim() === "") {
      onChange(undefined);
      onBlur?.();
      return;
    }

    const exactMatch = findExactMatch(search);
    if (exactMatch) {
      onChange(exactMatch);
    } else {
      onChange(undefined);
    }
    onBlur?.();
  };

  return (
    <div className="flex flex-col relative">
      <div
        className={`flex ${visibleError ? "justify-between" : "justify-start"}`}
      >
        <label className="text-md" htmlFor={id}>
          {label}
        </label>
        {visibleError && <span className="text-red-500">{visibleError}</span>}
      </div>
      <input
        className="border p-2 rounded border-gray-300"
        type="text"
        id={id}
        value={search}
        placeholder={placeholder}
        required={isRequired}
        onFocus={() => {
          setIsFocused(true);
          setIsOpen(true);
          setHighlightedIndex(-1);
        }}
        onChange={(e) => {
          setSearch(e.target.value);
          setIsOpen(true);
          setHighlightedIndex(-1);

          onChange(undefined);
        }}
        onBlur={handleBlur}
        onKeyDown={(e) => {
          if (e.key === "ArrowDown") {
            e.preventDefault();

            setIsOpen(true);

            setHighlightedIndex((current) =>
              current < filteredOptions.length - 1 ? current + 1 : 0,
            );
          }

          if (e.key === "ArrowUp") {
            e.preventDefault();

            setIsOpen(true);

            setHighlightedIndex((current) =>
              current > 0 ? current - 1 : filteredOptions.length - 1,
            );
          }

          if (e.key === "Enter") {
            e.preventDefault();

            if (
              isOpen &&
              highlightedIndex >= 0 &&
              filteredOptions[highlightedIndex]
            ) {
              handleSelect(filteredOptions[highlightedIndex]);
              return;
            }

            const exactMatch = findExactMatch(search);
            if (exactMatch) {
              handleSelect(exactMatch);
            } else {
              onChange(undefined);
              setIsOpen(false);
            }
          }

          if (e.key === "Escape") {
            setIsOpen(false);
            setHighlightedIndex(-1);
          }
        }}
      />

      {isOpen && filteredOptions.length > 0 && (
        <ul className="absolute top-full left-0 right-0 z-10 border border-gray-300 rounded mt-1 max-h-60 overflow-y-auto bg-white">
          {filteredOptions.map((option, index) => (
            <li
              key={`${option.label}_${option.id}`}
              ref={(element) => {
                optionRefs.current[index] = element;
              }}
              className={`p-2 cursor-pointer ${
                index === highlightedIndex ? "bg-gray-200" : "hover:bg-gray-200"
              }`}
              onMouseEnter={() => setHighlightedIndex(index)}
              onMouseDown={(e) => {
                e.preventDefault();
                handleSelect(option);
              }}
            >
              {option.label}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default SearchableSelect;
