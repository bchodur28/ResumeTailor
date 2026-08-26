import { useForm } from "react-hook-form";
import {
  extractionTypes,
  type ExtractionType,
} from "../../extraction/types/definitions/ExtractionType";
import type { FieldPattern } from "../../extraction/types/definitions/FieldPattern";
import {
  jobFieldNames,
  type JobFieldName,
} from "../../extraction/types/definitions/JobFieldName";
import Selector from "./UpdateSelectorForm";
import type { FieldExtractionDefinition } from "../../extraction/types/definitions/FieldExtractionDefinition";
import { useMutation } from "@tanstack/react-query";
import { updateFieldExtractionDefinition } from "../../api/extraction";
import type { FieldExtractionDefinitionRequest } from "../../extraction/types/requests/FieldExtractionDefinitionRequest";
import CreateSelectorForm from "./CreateSelectorForm";

type UpdateFieldFormProps = {
  id: number;
  siteId: number;
  fieldName: JobFieldName;
  displayLabel: string;
  extractionType: ExtractionType;
  attributeName: string | null;
  isRequired: boolean;
  sortOrder: number;
  selectors: FieldPattern[];
};

const UpdateFieldForm = ({
  id,
  siteId,
  fieldName,
  displayLabel,
  extractionType,
  attributeName,
  isRequired,
  sortOrder,
  selectors,
}: UpdateFieldFormProps) => {
  const { register, handleSubmit } = useForm<FieldExtractionDefinition>({
    mode: "onSubmit",
    defaultValues: {
      fieldName: fieldName,
      displayLabel: displayLabel,
      extractionType: extractionType,
      attributeName: attributeName,
      isRequired: isRequired,
      sortOrder: sortOrder,
    },
  });

  const updateFieldDefinitionMutation = useMutation({
    mutationFn: (field: FieldExtractionDefinitionRequest) =>
      updateFieldExtractionDefinition(id, field),
    onSuccess: () => {
      console.log("Field definition updated successfully");
    },

    onError: (error) => {
      console.error("Failed to update field definition:", error);
    },
  });

  const onSubmit = (field: FieldExtractionDefinition) => {
    const request: FieldExtractionDefinitionRequest = {
      siteExtractionDefinitionId: siteId,
      fieldName: field.fieldName,
      displayLabel: field.displayLabel,
      extractionType: field.extractionType,
      attributeName: field.attributeName,
      isRequired: field.isRequired,
      sortOrder: field.sortOrder,
    };
    updateFieldDefinitionMutation.mutate(request);
  };

  return (
    <form className="field-container" onSubmit={handleSubmit(onSubmit)}>
      <div className="input-container">
        <label htmlFor={`fieldName_${id}`}>Field Name:</label>
        <select
          id={`fieldName_${id}`}
          {...register("fieldName", { required: true })}
        >
          {jobFieldNames.map((name) => (
            <option key={name} value={name}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className="input-container">
        <label htmlFor={`displayLabel_${id}`}>Display Label:</label>
        <input
          id={`displayLabel_${id}`}
          {...register("displayLabel", { required: true })}
        />
      </div>

      <div className="input-container">
        <label htmlFor={`extractionType_${id}`}>Field Name:</label>
        <select
          id={`extractionType_${id}`}
          {...register("extractionType", { required: true })}
        >
          {extractionTypes.map((name) => (
            <option key={name} value={name}>
              {name}
            </option>
          ))}
        </select>
      </div>

      <div className="input-container">
        <label htmlFor={`attributeName_${id}`}>Attribute Name:</label>
        <input id={`attributeName_${id}`} {...register("attributeName")} />
      </div>

      <div className="input-container">
        <label htmlFor={`isRequired_${id}`}>Is Required:</label>
        <input
          type="checkbox"
          id={`isRequired_${id}`}
          {...register("isRequired")}
        />
      </div>

      <div className="input-container">
        <label htmlFor={`sortOrder_${id}`}>Sort Order:</label>
        <input
          type="number"
          id={`sortOrder_${id}`}
          {...register("sortOrder", { required: true, valueAsNumber: true })}
        />
      </div>

      <div className="selector-container">
        <p>Create New Selector:</p>
        <CreateSelectorForm fieldExtractionDefintionId={id} />
      </div>

      <div className="selector-container">
        <p>Existing Selectors:</p>
        {selectors.map((selector) => {
          return (
            <Selector
              key={selector.id}
              id={selector.id}
              fieldExtractionDefinitionId={id}
              rule={selector.matchPattern}
              priority={selector.priority}
            />
          );
        })}
      </div>

      <div className="submit-btn-container">
        <button
          type="submit"
          className="submit-btn"
          disabled={updateFieldDefinitionMutation.isPending}
        >
          {updateFieldDefinitionMutation.isPending
            ? "Updating..."
            : "Update Field"}
        </button>

        {updateFieldDefinitionMutation.isError && (
          <p>
            {updateFieldDefinitionMutation.error instanceof Error
              ? updateFieldDefinitionMutation.error.message
              : "The field could not be updated."}
          </p>
        )}
      </div>
    </form>
  );
};

export default UpdateFieldForm;
