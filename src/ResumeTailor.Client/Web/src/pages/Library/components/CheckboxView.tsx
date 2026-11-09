import type { UseFormRegisterReturn } from "react-hook-form";
import Checkbox from "../../../components/forms/Checkbox";

type CheckboxViewProps = {
  id: string;
  label: string;
  isInput: boolean;
  valueToDisplay?: string | null;
  registration?: UseFormRegisterReturn;
  error?: string;
};

const CheckboxView = ({
  id,
  label,
  isInput,
  valueToDisplay,
  registration,
}: CheckboxViewProps) => {
  return (
    <>
      {isInput && (
        <Checkbox
          id={id}
          label={label}
          registration={registration}
          labelOnLeft={true}
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

export default CheckboxView;
