import { useFormContext, useFormState, useWatch } from "react-hook-form";
import Button from "../../../components/ui/buttons/Button";
import {
  CheckCircle,
  ExclamationCircle,
  Trash3,
  ArrowCounterclockwise,
} from "react-bootstrap-icons";
import TextArea from "../../../components/forms/TextArea";
import type { LibraryExperienceRequest } from "../../../api/contracts/library/LibraryExperienceRequest";
import SlideoutMessage from "../../../components/ui/SlideoutMessage";

type SummaryEditorProps = {
  index: number;
  onDelete: (index: number) => void;
};

const SummaryEditor = ({ index, onDelete }: SummaryEditorProps) => {
  const { control, register, resetField, getValues } =
    useFormContext<LibraryExperienceRequest>();

  const fieldName = `summaries.${index}` as const;

  const { errors, dirtyFields } = useFormState({
    control,
    name: fieldName,
    exact: false,
  });

  const isDeleting = useWatch({
    control,
    name: `summaries.${index}.isDeleting`,
  });

  const summary = getValues(fieldName);

  const isUnsaved = summary?.id == null || !!dirtyFields.summaries?.[index];

  return (
    <div
      className={`flex flex-col gap-2 border p-2 rounded-lg ${
        isDeleting ? "border-red-500 border-2" : "border-gray-300"
      }`}
    >
      <TextArea
        id={`summary-${index}-value`}
        label={`${index === 0 ? "Primary Summary" : `Summary ${index + 1}`}`}
        registration={register(`summaries.${index}.value`, {
          required: "Summary is required",
        })}
        error={errors.summaries?.[index]?.value?.message}
      />

      <div className="flex justify-between items-center">
        <SlideoutMessage
          messsageType={isUnsaved ? "neutral" : "positive"}
          message={isUnsaved ? "Unsaved" : "Saved"}
        />

        <div className="flex gap-2">
          {!isDeleting && summary?.id != null && (
            <Button
              label="Flag for Deletion"
              onClick={() => onDelete(index)}
              iconComponent={Trash3}
              styleType="plain"
              labelSlideOut
            />
          )}

          {isUnsaved && (
            <Button
              label={
                summary?.id == null
                  ? "Remove New Summary"
                  : "Revert Summary Change"
              }
              onClick={() => {
                if (summary?.id == null) {
                  onDelete(index);
                } else {
                  resetField(`summaries.${index}`);
                }
              }}
              iconComponent={ArrowCounterclockwise}
              styleType="plain"
              labelSlideOut
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default SummaryEditor;
