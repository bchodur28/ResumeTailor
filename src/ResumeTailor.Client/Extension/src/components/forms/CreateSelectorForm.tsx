import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createFieldSelector, updateFieldSelector } from "../../api/extraction";
import type { FieldPatternRequest } from "../../extraction/types/requests/FieldPattternRequest";
import { useForm } from "react-hook-form";
import type { FieldPattern } from "../../extraction/types/definitions/FieldPattern";

type CreateSelectorFormProps = {
  fieldExtractionDefintionId: number;
};

const CreateSelectorForm = ({
  fieldExtractionDefintionId,
}: CreateSelectorFormProps) => {
  const queryClient = useQueryClient();
  const { register, handleSubmit } = useForm<FieldPattern>({
    defaultValues: {
      matchPattern: "",
      scopePattern: "",
      priority: 0,
    },
  });

  const createSelectorMutation = useMutation({
    mutationFn: createFieldSelector,
    onSuccess: async (createdFieldSelector: FieldPattern) => {
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

  const onSubmit = (selector: FieldPattern) => {
    const request: FieldPatternRequest = {
      fieldExtractionDefinitionId: fieldExtractionDefintionId,
      matchPattern: selector.matchPattern,
      scopePattern: selector.scopePattern,
      priority: selector.priority,
    };
    createSelectorMutation.mutate(request);
  };

  return (
    <form className="field-container" onSubmit={handleSubmit(onSubmit)}>
      <div className="input-container">
        <label htmlFor={`rule_0`}>Selector Rule:</label>
        <input id={`rule_0`} {...register("matchPattern")} />
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
