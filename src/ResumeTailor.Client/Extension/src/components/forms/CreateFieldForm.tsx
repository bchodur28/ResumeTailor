import { useForm } from "react-hook-form";
import { extractionTypes } from "../../extraction/types/definitions/ExtractionType";
import { jobFieldNames } from "../../extraction/types/definitions/JobFieldName";
import type { FieldExtractionDefinition } from "../../extraction/types/definitions/FieldExtractionDefinition";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { FieldExtractionDefinitionRequest } from "../../extraction/types/requests/FieldExtractionDefinitionRequest";
import { createFieldExtractionDefinition } from "../../api/extraction";

type CreateFieldFormProps = {
  siteId: number;
};

const CreateFieldForm = ({ siteId }: CreateFieldFormProps) => {
  const queryClient = useQueryClient();
  const { register, handleSubmit } = useForm<FieldExtractionDefinition>({
    mode: "onSubmit",
    defaultValues: {
      fieldName: "jobTitle",
      displayLabel: "",
      extractionType: "text",
      attributeName: null,
      isRequired: true,
      sortOrder: 0,
    },
  });

  const createFieldDefinitionMutation = useMutation({
    mutationFn: createFieldExtractionDefinition,
    onSuccess: async (createdFieldDefinition) => {
      console.log(
        "Field definition created successfully:",
        createdFieldDefinition,
      );
      await queryClient.invalidateQueries({
        queryKey: ["siteExtractionDefinition"],
      });
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
    createFieldDefinitionMutation.mutate(request);
  };

  return (
    <form className="field-container" onSubmit={handleSubmit(onSubmit)}>
      <div className="input-container">
        <label htmlFor={`fieldName_0`}>Field Name:</label>
        <select
          id={`fieldName_0`}
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
        <label htmlFor={`displayLabel_0`}>Display Label:</label>
        <input
          id={`displayLabel_0`}
          {...register("displayLabel", { required: true })}
        />
      </div>

      <div className="input-container">
        <label htmlFor={`extractionType_0`}>Extraction Type:</label>
        <select
          id={`extractionType_0`}
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
        <label htmlFor={`attributeName_0`}>Attribute Name:</label>
        <input id={`attributeName_0`} {...register("attributeName")} />
      </div>

      <div className="input-container">
        <label htmlFor={`isRequired_0`}>Is Required:</label>
        <input
          type="checkbox"
          id={`isRequired_0`}
          {...register("isRequired")}
        />
      </div>

      <div className="input-container">
        <label htmlFor={`sortOrder_0`}>Sort Order:</label>
        <input
          type="number"
          id={`sortOrder_0`}
          {...register("sortOrder", { required: true, valueAsNumber: true })}
        />
      </div>

      {/* <div className="selector-container">
        <p>Selectors:</p>
        {selectors.map((selector) => {
          return (
            <Selector
              key={selector.id}
              id={selector.id}
              rule={selector.selector}
              priority={selector.priority}
            />
          );
        })}
      </div> */}

      <div className="submit-btn-container">
        <button
          type="submit"
          className="submit-btn"
          disabled={createFieldDefinitionMutation.isPending}
        >
          {createFieldDefinitionMutation.isPending
            ? "Creating..."
            : "Create Field"}
        </button>

        {createFieldDefinitionMutation.isError && (
          <p>
            {createFieldDefinitionMutation.error instanceof Error
              ? createFieldDefinitionMutation.error.message
              : "The field could not be updated."}
          </p>
        )}
      </div>
    </form>
  );
};

export default CreateFieldForm;
