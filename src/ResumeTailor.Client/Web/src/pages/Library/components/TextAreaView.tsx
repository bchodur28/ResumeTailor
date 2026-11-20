import type { UseFormRegisterReturn } from "react-hook-form";
import TextArea from "../../../components/forms/TextArea";
import { useState } from "react";

type TextAreaViewProps = {
  id: string;
  label: string;
  isInput: boolean;
  type?: string;
  valueToDisplay?: string | null;
  registration?: UseFormRegisterReturn;
  error?: string;
  isValid?: boolean;
  rows?: number;
  textOnNextRow?: boolean;
};

const truncateWords = (str: string) => {
  const words = str.trim().split(/\s+/);
  if (words.length <= 30) {
    return str;
  }
  return words.slice(0, 30).join(" ") + "...";
};

const TextAreaView = ({
  id,
  label,
  isInput,
  valueToDisplay,
  registration,
  error,
  rows,
  textOnNextRow = false,
}: TextAreaViewProps) => {
  const [isTruncated, setIsTruncated] = useState(true);

  const toggleText = () => {
    setIsTruncated(!isTruncated);
  };

  const filteredValue = isTruncated
    ? truncateWords(valueToDisplay ?? "")
    : (valueToDisplay ?? "");

  return (
    <>
      {isInput && (
        <TextArea
          id={id}
          label={label}
          registration={registration}
          error={error}
          rows={rows}
        />
      )}
      {!isInput && (
        <li
          className={`flex ${textOnNextRow ? "flex-col" : "gap-2"}  items-baseline`}
        >
          <p className="text-sm font-medium text-gray-600">{label}:</p>
          <p
            onClick={() => {
              toggleText();
            }}
            className="text-base font-medium text-gray-700 cursor-pointer"
          >
            {filteredValue}
          </p>
        </li>
      )}
    </>
  );
};

export default TextAreaView;
