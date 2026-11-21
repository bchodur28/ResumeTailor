import type { UseFormRegisterReturn } from "react-hook-form";

type InputProps = {
  id: string;
  label: string;
  name?: string;
  checked?: boolean;
  onChange?: (checked: boolean) => void;
  registration?: UseFormRegisterReturn;
};

const Radio = ({
  id,
  label,
  name,
  checked,
  registration,
  onChange,
}: InputProps) => {
  return (
    <div className="flex items-center gap-2">
      <input
        className="h-4 w-4 cursor-pointer translate-y-0.5"
        type="radio"
        id={id}
        name={name}
        {...registration}
        checked={checked}
        onChange={(e) => {
          registration?.onChange?.(e);
          onChange?.(e.target.checked);
        }}
      />
      <label className="text-gray-500 text-sm font-medium" htmlFor={id}>
        {label}
      </label>
    </div>
  );
};

export default Radio;
