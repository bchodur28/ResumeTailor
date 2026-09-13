import type { Option } from "../../models/forms/Option";
import { useEffect, useState } from "react";

type SearchableSelectProps = {
  id: string;
  options: Option[];
  onChange: (option: Option) => void;
  onInvalid?: () => void;
  value?: Option;
  placeholder?: string;
  label?: string;
  isRequired?: boolean;
};

const SearchableSelect = ({
  id,
  options,
  onChange,
  onInvalid,
  value,
  placeholder,
  label,
  isRequired,
}: SearchableSelectProps) => {
  const [search, setSearch] = useState("");
  const [isOpen, setIsOpen] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [hasTouched, setHasTouched] = useState(false);

  useEffect(() => {
    setSearch(value?.label ?? "");
  }, [value]);

  const filteredOptions = options.filter((option) =>
    option.label.toLowerCase().includes(search.toLowerCase()),
  );

  const handleSelect = (option: Option) => {
    setSearch(option.label);
    setIsOpen(false);
    setError(null);
    onChange(option);
  };

  const validate = () => {
    if (isRequired && hasTouched && search.trim() === "") {
      setError("This field is required");
      onInvalid?.();
      return false;
    }

    if (isRequired && !hasTouched && search.trim() === "") {
      onInvalid?.();
      return false;
    }

    if (!isRequired || search.trim() !== "") {
      return true;
    }

    const exactMatch = options.find(
      (option) => option.label.toLowerCase() === search.toLowerCase(),
    );

    if (!exactMatch) {
      setError("Please select a valid option");
      onInvalid?.();
      return false;
    }

    setError(null);
    onChange(exactMatch);

    return true;
  };

  return (
    <div className="flex flex-col relative">
      <div className={`flex ${error ? "justify-between" : "justify-start"}`}>
        <label className="text-lg" htmlFor={id}>
          {label}
        </label>
        {error && <span className="text-red-500">{error}</span>}
      </div>
      <input
        className="border p-2 rounded border-gray-300"
        type="text"
        id={id}
        value={search}
        placeholder={placeholder}
        onFocus={() => setIsOpen(true)}
        onChange={(e) => {
          setSearch(e.target.value);
          setHasTouched(true);
          setIsOpen(true);
          setError(null);
        }}
        onBlur={() => {
          setIsOpen(false);
          validate();
        }}
        onKeyDown={(e) => {
          if (e.key === "Enter") {
            e.preventDefault();
            validate();
            setIsOpen(false);
          }
        }}
      />

      {isOpen && filteredOptions.length > 0 && (
        <ul className="absolute top-full left-0 right-0 z-10 border border-gray-300 rounded mt-1 max-h-60 overflow-y-auto bg-white">
          {filteredOptions.map((option) => (
            <li
              key={`${option.label} _ ${option.id}`}
              className="p-2 cursor-pointer hover:bg-gray-200"
              onMouseDown={() => handleSelect(option)}
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
