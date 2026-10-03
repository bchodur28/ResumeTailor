import type { UseFormRegisterReturn } from "react-hook-form";
import { useState } from "react";
import TextArea from "./TextArea";
import { Trash3 } from "react-bootstrap-icons";

export type ToggleableViewTextArea = {
  id: string;
  label: string;
  value?: string;
  resumeCount?: number;
  registration?: UseFormRegisterReturn;
  additionalButtons?: React.ReactNode[];
  error?: string;
  placeholder?: string;
  onFocus?: () => void;
  onBlur?: () => void;
  rows?: number;
};

const ToggleableViewTextArea = ({
  id,
  label,
  value,
  resumeCount,
  registration,
  additionalButtons,
  error,
  placeholder,
  onFocus,
  onBlur,
  rows,
}: ToggleableViewTextArea) => {
  const [isEditing, setIsEditing] = useState(false);
  return (
    <div className="flex flex-1">
      <div className="flex flex-1 p-4 border border-gray-300 rounded-md bg-gray-100">
        <div className="flex flex-1 items-center gap-2 mr-4">
          {!isEditing && (
            <div>
              <p className="whitespace-pre-wrap font-semibold">{value}</p>
              {resumeCount !== undefined && (
                <p className="whitespace-pre-wrap font-semibold text-sm text-gray-600">
                  Resume Count: {resumeCount}
                </p>
              )}
            </div>
          )}
          {isEditing && (
            <TextArea
              id={id}
              label={label}
              registration={registration}
              error={error}
              placeholder={placeholder}
              onFocus={onFocus}
              onBlur={onBlur}
              rows={rows}
            />
          )}
        </div>
        <div className="flex gap-2 items-start">
          <button
            type="button"
            className="btn-secondary"
            onClick={() => setIsEditing(!isEditing)}
          >
            {isEditing ? "View" : "Edit"}
          </button>
          {additionalButtons?.map((button, index) => (
            <div className="flex" key={index}>
              {button}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ToggleableViewTextArea;
