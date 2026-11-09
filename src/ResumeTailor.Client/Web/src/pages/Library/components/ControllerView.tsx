import {
  Controller,
  useWatch,
  type Control,
  type FieldValues,
  type FieldPathByValue,
} from "react-hook-form";

import type { Option } from "../../../models/forms/Option";
import Select from "../../../components/forms/Select";

type OptionId = Option["id"];

type ControllerViewProps<T extends FieldValues> = {
  label: string;
  name: FieldPathByValue<T, OptionId | undefined>;
  isInput: boolean;
  isRequired: boolean;
  options: Option[];
  control: Control<T>;
};

const ControllerView = <T extends FieldValues>({
  label,
  name,
  isInput,
  isRequired,
  options,
  control,
}: ControllerViewProps<T>) => {
  const selectedValue = useWatch({
    control,
    name,
  });

  const selectedOption = options.find((option) => option.id === selectedValue);

  return (
    <div>
      {isInput ? (
        <Controller
          name={name}
          control={control}
          rules={{
            required: isRequired ? "This field is required." : false,
          }}
          render={({ field, fieldState }) => (
            <Select
              id={field.name}
              label={label}
              options={options}
              value={options.find((option) => option.id === field.value)}
              onChange={(option) => {
                field.onChange(option?.id);
              }}
              onBlur={field.onBlur}
              isRequired={isRequired}
              error={fieldState.error?.message}
            />
          )}
        />
      ) : (
        <div className="flex gap-2 items-baseline">
          <p className="text-sm font-medium text-gray-500">{label}:</p>
          <p className="text-base font-medium text-gray-700">
            {selectedOption?.label ?? ""}
          </p>
        </div>
      )}
    </div>
  );
};

export default ControllerView;
