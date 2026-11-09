import type { UseFormRegisterReturn } from "react-hook-form";
import Input from "../../../components/forms/Input";
import { formatDate } from "../../../models/common/DateFormat";

type InputViewProps = {
  id: string;
  label: string;
  isInput: boolean;
  type?: string;
  valueToDisplay?: string | null;
  registration?: UseFormRegisterReturn;
  error?: string;
  isValid?: boolean;
  dateFormatterFn?: (date: string | null | undefined) => string;
};

const InputView = ({
  id,
  label,
  isInput,
  type,
  valueToDisplay,
  registration,
  error,
  isValid,
  dateFormatterFn,
}: InputViewProps) => {
  return (
    <>
      {isInput && (
        <Input
          id={id}
          label={label}
          type={type}
          registration={registration}
          error={error}
          isValid={isValid}
        />
      )}
      {!isInput && (
        <li className="flex gap-2 items-baseline">
          <p className="text-sm font-medium text-gray-600">{label}:</p>
          <p className="text-base font-medium text-gray-700">
            {type === "date" && dateFormatterFn
              ? dateFormatterFn(valueToDisplay)
              : (valueToDisplay ?? "")}
          </p>
        </li>
      )}
    </>
  );
};

export default InputView;
