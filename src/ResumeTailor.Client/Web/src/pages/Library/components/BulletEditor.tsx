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

type BulletEditorProps = {
  index: number;
  onDelete: (index: number) => void;
};

const BulletEditor = ({ index, onDelete }: BulletEditorProps) => {
  const { control, register, resetField, getValues } =
    useFormContext<LibraryExperienceRequest>();

  const fieldName = `bullets.${index}` as const;

  const { errors, dirtyFields } = useFormState({
    control,
    name: fieldName,
    exact: false,
  });

  const isDeleting = useWatch({
    control,
    name: `bullets.${index}.isDeleting`,
  });

  const bullet = getValues(fieldName);

  const isUnsaved = bullet?.id == null || !!dirtyFields.bullets?.[index];

  return (
    <div
      className={`flex flex-col gap-2 border p-2 rounded-lg ${
        isDeleting ? "border-red-500 border-2" : "border-gray-300"
      }`}
    >
      <TextArea
        id={`bullet-${index}-value`}
        label={`Bullet ${index + 1}`}
        registration={register(`bullets.${index}.value`, {
          required: "Bullet is required",
        })}
        error={errors.bullets?.[index]?.value?.message}
      />

      <div className="flex justify-between items-center">
        <SlideoutMessage
          messsageType={isUnsaved ? "neutral" : "positive"}
          message={isUnsaved ? "Unsaved" : "Saved"}
        />

        <div className="flex gap-2">
          {!isDeleting && bullet?.id != null && (
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
                bullet?.id == null
                  ? "Remove New Bullet"
                  : "Revert Bullet Change"
              }
              onClick={() => {
                if (bullet?.id == null) {
                  onDelete(index);
                } else {
                  resetField(`bullets.${index}`);
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

export default BulletEditor;
