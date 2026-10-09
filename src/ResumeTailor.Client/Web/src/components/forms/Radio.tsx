import type { UseFormRegisterReturn } from "react-hook-form";

type InputProps = {
  id: string;
  label: string;
  registration?: UseFormRegisterReturn;
};

const Radio = ({ id, label, registration }: InputProps) => {
  return (
    <div className="flex items-center gap-2">
      <input
        className="h-4 w-4 cursor-pointer translate-y-0.5"
        type="radio"
        id={id}
        {...registration}
      />
      <label className="text-sm font-semibold text-gray-700" htmlFor={id}>
        {label}
      </label>
    </div>
  );
};

export default Radio;
