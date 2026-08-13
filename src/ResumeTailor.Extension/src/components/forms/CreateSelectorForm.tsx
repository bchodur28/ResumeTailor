import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFieldSelector, updateFieldSelector } from "../../api/extraction";
import type { FieldSelectorRequest } from "../../extraction/types/requests/FieldSelectorRequest";
import { useForm } from "react-hook-form";
import type { FieldSelector } from "../../extraction/types/definitions/FieldSelector";

type CreateSelectorFormProps = {
  fieldExtractionDefintionId: number;
};

const CreateSelectorForm = ({
  fieldExtractionDefintionId,
}: CreateSelectorFormProps) => {
  const queryClient = useQueryClient();
  const { register, handleSubmit } = useForm<FieldSelector>({
    defaultValues: {
      selector: "",
      priority: 0,
    },
  });

  const createSelectorMutation = useMutation({
    mutationFn: createFieldSelector,
    onSuccess: async (createdFieldSelector: FieldSelector) => {
      console.log("Selector created successfully:", createdFieldSelector);
      // Invalidate the query to refresh the list of selectors
      await queryClient.invalidateQueries({
        queryKey: ["siteExtractionDefinitions"],
      });
    },
    onError: (error) => {
      console.error("Failed to create selector:", error);
    },
  });

  const onSubmit = (selector: FieldSelector) => {
    const request: FieldSelectorRequest = {
      fieldExtractionDefinitionId: fieldExtractionDefintionId,
      selector: selector.selector,
      priority: selector.priority,
    };
    createSelectorMutation.mutate(request);
  };

  return (
    <form className="field-container" onSubmit={handleSubmit(onSubmit)}>
      <div className="input-container">
        <label htmlFor={`rule_0`}>Selector Rule:</label>
        <input id={`rule_0`} {...register("selector")} />
      </div>

      <div className="input-container">
        <label htmlFor={`priority_0`}>priority:</label>
        <input
          type="number"
          id={`priority_0`}
          {...register("priority", { valueAsNumber: true })}
        />
      </div>
      <div className="submit-btn-container">
        <button
          className="submit-btn"
          disabled={createSelectorMutation.isPending}
        >
          {createSelectorMutation.isPending ? "creating..." : "Create Selector"}
        </button>
      </div>
    </form>
  );
};

export default CreateSelectorForm;
