import type { UseFormRegisterReturn } from "react-hook-form";
import { useState } from "react";
import { Check2Circle, CheckCircleFill } from "react-bootstrap-icons";

type InputProps = {
  id: string;
  label: string;
  registration?: UseFormRegisterReturn;
  error?: string;
  placeholder?: string;
  type?: string;
  isValid?: boolean;
  onFocus?: () => void;
  onBlur?: () => void;
  optionalText?: string;
};

const Input = ({
  id,
  label,
  registration,
  error,
  placeholder,
  type = "text",
  isValid,
  onFocus,
  onBlur,
  optionalText,
}: InputProps) => {
  return (
    <div className="flex flex-col flex-1">
      <div
        className={`flex ${error || isValid ? "justify-between" : "justify-start"}`}
      >
        <label className="text-sm font-medium text-gray-500" htmlFor={id}>
          {label}
          {optionalText && (
            <span className="ml-1 text-gray-500 text-sm font-regular">{`(${optionalText})`}</span>
          )}
        </label>
        {error && (
          <span className="text-red-500 text-sm font-medium">{error}</span>
        )}
        {isValid && (
          <span className="text-green-500 mt-1">
            <Check2Circle />
          </span>
        )}
      </div>
      <input
        className={`border p-2 rounded border-gray-300`}
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
