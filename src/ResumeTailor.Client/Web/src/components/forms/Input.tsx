import type { UseFormRegisterReturn } from "react-hook-form";
import { useState } from "react";

type InputProps = {
  id: string;
  label: string;
  registration?: UseFormRegisterReturn;
  error?: string;
  placeholder?: string;
  type?: string;
  onFocus?: () => void;
  onBlur?: () => void;
};

const Input = ({
  id,
  label,
  registration,
  error,
  placeholder,
  type = "text",
  onFocus,
  onBlur,
}: InputProps) => {
  return (
    <div className="flex flex-col flex-1">
      <div className={`flex ${error ? "justify-between" : "justify-start"}`}>
        <label className="text-md" htmlFor={id}>
          {label}
        </label>
        {error && <span className="text-red-500">{error}</span>}
      </div>
      <input
        className="border p-2 rounded border-gray-300"
        placeholder={placeholder}
        type={type}
        id={id}
        {...registration}
        onFocus={() => {
          onFocus?.();
        }}
        onBlur={(e) => {
          registration?.onBlur?.(e);
          onBlur?.();
        }}
      />
    </div>
  );
};

export default Input;
