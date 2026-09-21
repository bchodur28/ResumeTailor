import { useAuth0 } from "@auth0/auth0-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEducation } from "../../hooks/useEducation";
import { useFieldArray, useForm } from "react-hook-form";
import { useState } from "react";
import type { EducationForm } from "../../models/forms/EducationForm";
import { useEffect } from "react";
import type { EducationRequest } from "../../models/api/EducationRequest";
import Input from "../forms/Input";
import { Trash3 } from "react-bootstrap-icons";
import {
  createEducation,
  updateEducation,
  deleteEducation,
} from "../../api/education";
import Checkbox from "../forms/Checkbox";
import Message from "../ui/Message";

const EditEducationForm = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { data: education, isLoading, isError } = useEducation();

  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors, isValid },
  } = useForm<EducationForm>({
    defaultValues: {
      education: education ?? [],
    },
  });

  const {
    fields: educationFields,
    append: appendEducaiton,
    remove: removeEducation,
  } = useFieldArray({
    control,
    name: "education",
  });

  useEffect(() => {
    if (!education) {
      return;
    }

    reset({
      education: education.map((item) => ({
        id: item.id,
        schoolName: item.schoolName,
        degree: item.degree,
        major: item.major,
        started: item.started,
        ended: item.ended,
        useForResume: item.useForResume,
      })),
    });
  }, [education, reset]);

  const saveEducationMutation = useMutation({
    mutationFn: async (data: EducationForm) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });

      const normalizedData = data.education.map((item) => ({
        ...item,
        ended: item.ended === "" ? null : item.ended,
      }));

      const existingEducation = education ?? [];

      const educationToCreate: EducationRequest[] = normalizedData.filter(
        (item) => item.id == null,
      );

      const educationToUpdate: EducationRequest[] = normalizedData.filter(
        (item) => item.id != null,
      );

      const submittedIds = new Set(normalizedData.map((item) => item.id!));

      const educationIdsToDelete = existingEducation
        .filter((item) => !submittedIds.has(item.id))
        .map((item) => item.id);

      const requests: Promise<void>[] = [];

      if (educationToCreate.length > 0) {
        requests.push(createEducation(educationToCreate, token));
      }
      if (educationToUpdate.length > 0) {
        requests.push(updateEducation(educationToUpdate, token));
      }
      if (educationIdsToDelete.length > 0) {
        requests.push(deleteEducation(educationIdsToDelete, token));
      }
      await Promise.all(requests);
    },
    onSuccess: () => {
      setIsSavedSuccessfully(true);
      queryClient.invalidateQueries({ queryKey: ["education"] });
    },
    onError: (error: String) => {
      setIsSavedSuccessfully(false);
      console.error("Failed to save education:", error);
    },
  });

  const handleAddEducation = () => {
    appendEducaiton({
      schoolName: "",
      degree: "",
      major: "",
      started: "",
      ended: "",
      useForResume: false,
    });
  };

  const handleRemoveEducation = (index: number) => {
    removeEducation(index);
  };

  const onSubmitEducation = (data: EducationForm) => {
    saveEducationMutation.mutate(data);
  };

  if (isLoading) {
    return <div>Loading Education...</div>;
  }

  if (isError) {
    return <div>Failed to load education.</div>;
  }

  return (
    <form
      className="flex flex-col gap-2"
      onSubmit={handleSubmit(onSubmitEducation)}
    >
      {isSavedSuccessfully && (
        <Message
          type="success"
          message="Education saved successfully."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {isSavedSuccessfully === false && (
        <Message
          type="error"
          message="Failed to save education."
          onClose={() => setIsSavedSuccessfully(null)}
        />
      )}
      {educationFields.map((field, index) => (
        <div
          key={field.id}
          className="flex flex-col gap-4 border border-gray-300 p-4 rounded-md"
        >
          <div className="flex justify-between items-center">
            <h4 className="text-lg font-semibold">
              Education {educationFields.length > 1 ? index + 1 : ""}
            </h4>
            <button
              type="button"
              className="remove-btn"
              onClick={() => handleRemoveEducation(index)}
            >
              <Trash3 />
            </button>
          </div>

          <Input
            id={`education[${index}].schoolName`}
            label="School Name"
            registration={register(`education.${index}.schoolName`, {
              required: "This field is required.",
            })}
            error={errors.education?.[index]?.schoolName?.message}
          />
          <Input
            id={`education[${index}].degree`}
            label="Degree"
            registration={register(`education.${index}.degree`, {
              required: "This field is required.",
            })}
            error={errors.education?.[index]?.degree?.message}
          />
          <Input
            id={`education[${index}].major`}
            label="Major"
            registration={register(`education.${index}.major`, {
              required: "This field is required.",
            })}
            error={errors.education?.[index]?.major?.message}
          />
          <Input
            id={`education[${index}].started`}
            label="Started"
            registration={register(`education.${index}.started`, {
              required: "This field is required.",
            })}
            error={errors.education?.[index]?.started?.message}
            type="date"
          />
          <Input
            id={`education[${index}].ended`}
            label="Ended"
            registration={register(`education.${index}.ended`)}
            type="date"
          />
          <Checkbox
            id={`education[${index}].useForResume`}
            label="Use For Resume"
            registration={register(`education.${index}.useForResume`)}
          />
        </div>
      ))}
      {educationFields.length === 0 && (
        <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
          <p className="text-gray-700 font-semibold">
            No education entries. Click to add.
          </p>
        </div>
      )}
      <div className="flex justify-end gap-2">
        <button
          type="button"
          className="btn-secondary"
          onClick={handleAddEducation}
        >
          Add Education
        </button>
        <button type="submit" className="confirm-btn">
          Save Education
        </button>
      </div>
    </form>
  );
};

export default EditEducationForm;
