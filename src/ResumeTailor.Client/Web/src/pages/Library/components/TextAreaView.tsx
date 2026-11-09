import type { UseFormRegisterReturn } from "react-hook-form";
import TextArea from "../../../components/forms/TextArea";

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
};

const TextAreaView = ({
  id,
  label,
  isInput,
  valueToDisplay,
  registration,
  error,
  rows,
}: TextAreaViewProps) => {
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
        <li className="flex gap-2 items-baseline">
          <p className="text-sm font-medium text-gray-600">{label}:</p>
          <p className="text-base font-medium text-gray-700">
            {valueToDisplay ?? ""}
          </p>
        </li>
      )}
    </>
  );
};

export default TextAreaView;
