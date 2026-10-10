import type { UseFormRegisterReturn } from "react-hook-form";

type InputProps = {
  id: string;
  label: string;
  checked?: boolean;
  onChange?: (checked: boolean) => void;
  registration?: UseFormRegisterReturn;
};

const Checkbox = ({
  id,
  label,
  checked,
  registration,
  onChange,
}: InputProps) => {
  return (
    <div className="flex items-center gap-2">
      <input
        className="h-4 w-4 cursor-pointer translate-y-0.5"
        type="checkbox"
        id={id}
        {...registration}
        checked={checked}
        onChange={(e) => {
          registration?.onChange?.(e);
          onChange?.(e.target.checked);
        }}
      />
      <label className="text-sm font-semibold text-gray-700" htmlFor={id}>
        {label}
      </label>
    </div>
  );
};

export default Checkbox;
