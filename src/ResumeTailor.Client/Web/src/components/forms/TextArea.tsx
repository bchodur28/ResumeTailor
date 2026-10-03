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
}: TextAreaProps) => {
  return (
    <div className="flex flex-col flex-1">
      <div className={`flex ${error ? "justify-between" : "justify-start"}`}>
        <label className="text-md" htmlFor={id}>
          {label}
        </label>
        {error && <span className="text-red-500">{error}</span>}
      </div>
      <textarea
        className="border p-2 rounded border-gray-300 bg-white"
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
