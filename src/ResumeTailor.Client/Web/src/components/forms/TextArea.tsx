import type { UseFormRegisterReturn } from "react-hook-form";

type TextAreaProps = {
  id: string;
  label: string;
  registration?: UseFormRegisterReturn;
  error?: string;
  placeholder?: string;
  onFocus?: () => void;
  onBlur?: () => void;
  rows?: number;
  borderColorClass?: string;
  optionalText?: string;
};

const TextArea = ({
  id,
  label,
  registration,
  error,
  placeholder,
  onFocus,
  onBlur,
  rows,
  borderColorClass,
  optionalText,
}: TextAreaProps) => {
  return (
    <div className="flex flex-col flex-1">
      <div className={`flex ${error ? "justify-between" : "justify-start"}`}>
        <label className="text-sm font-medium text-gray-500" htmlFor={id}>
          {label}
          {optionalText && (
            <span className="ml-1 text-gray-500 text-sm font-regular">{`(${optionalText})`}</span>
          )}
        </label>
        {error && <span className="text-red-500">{error}</span>}
      </div>
      <textarea
        className={`border p-2 rounded bg-white ${borderColorClass ?? "border-gray-300"}`}
        placeholder={placeholder}
        id={id}
        {...registration}
        onFocus={() => {
          onFocus?.();
        }}
        onBlur={(e) => {
          registration?.onBlur?.(e);
          onBlur?.();
        }}
        rows={rows ?? 4}
      />
    </div>
  );
};

export default TextArea;
