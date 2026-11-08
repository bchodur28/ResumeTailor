import type { UseFormRegisterReturn } from "react-hook-form";

type InputProps = {
  id: string;
  label: string;
  checked?: boolean;
  onChange?: (checked: boolean) => void;
  registration?: UseFormRegisterReturn;
  labelOnLeft?: boolean;
};

const Checkbox = ({
  id,
  label,
  checked,
  registration,
  onChange,
  labelOnLeft = false,
}: InputProps) => {
  return (
    <div className="flex items-center gap-2">
      {labelOnLeft && (
        <label className="mr-1 text-gray-500 text-sm font-medium" htmlFor={id}>
          {label}
        </label>
      )}
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
      {labelOnLeft == false && (
        <label className="mr-1 text-gray-500 text-sm font-medium" htmlFor={id}>
          {label}
        </label>
      )}
    </div>
  );
};

export default Checkbox;
