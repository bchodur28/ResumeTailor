import { useMutation } from "@tanstack/react-query";
import { updateFieldSelector } from "../../api/extraction";
import type { FieldSelectorRequest } from "../../extraction/types/requests/FieldSelectorRequest";
import { useForm } from "react-hook-form";
import type { FieldSelector } from "../../extraction/types/definitions/FieldSelector";

type UpdateSelectorFormProps = {
  id: number;
  fieldExtractionDefinitionId: number;
  rule: string;
  priority: number;
};

const UpdateSelectorForm = ({
  id,
  fieldExtractionDefinitionId,
  rule,
  priority,
}: UpdateSelectorFormProps) => {
  const { register, handleSubmit } = useForm<FieldSelectorRequest>({
    defaultValues: {
      selector: rule,
      priority: priority,
    },
  });

  const updateSelectorMutation = useMutation({
    mutationFn: (selector: FieldSelectorRequest) =>
      updateFieldSelector(id, selector),
    onSuccess: () => {
      console.log("Selector updated successfully");
    },
    onError: (error) => {
      console.error("Failed to update selector:", error);
    },
  });

  const onSubmit = (fieldSelector: FieldSelector) => {
    const request: FieldSelectorRequest = {
      fieldExtractionDefinitionId: fieldExtractionDefinitionId,
      selector: fieldSelector.selector,
      priority: fieldSelector.priority,
    };
    updateSelectorMutation.mutate(request);
  };

  return (
    <form className="field-container">
      <div className="input-container">
        <label htmlFor={`rule_${id}`}>Selector Rule:</label>
        <input id={`rule_${id}`} {...register("selector")} />
      </div>

      <div className="input-container">
        <label htmlFor={`priority_${id}`}>priority:</label>
        <input
          type="number"
          id={`priority_${id}`}
          {...register("priority", { valueAsNumber: true })}
        />
      </div>
      <div className="submit-btn-container">
        <button
          className="submit-btn"
          disabled={updateSelectorMutation.isPending}
        >
          {updateSelectorMutation.isPending ? "Updating..." : "Update Selector"}
        </button>
      </div>
    </form>
  );
};

export default UpdateSelectorForm;
