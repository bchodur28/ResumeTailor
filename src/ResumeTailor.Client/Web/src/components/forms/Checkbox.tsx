import type { UseFormRegisterReturn } from "react-hook-form";
import { useState } from "react";

type InputProps = {
  id: string;
  label: string;
  registration?: UseFormRegisterReturn;
};

const Checkbox = ({ id, label, registration }: InputProps) => {
  return (
    <div className="flex items-center gap-2">
      <label className="text-md " htmlFor={id}>
        {label}
      </label>
      <input
        className="h-4 w-4 cursor-pointer translate-y-0.5"
        type="checkbox"
        id={id}
        {...registration}
      />
    </div>
  );
};

export default Checkbox;
