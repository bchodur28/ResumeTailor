import { PencilSquare, Trash3 } from "react-bootstrap-icons";
import type { EducationRequest } from "../../api/contracts/education/EducationRequest";
import Checkbox from "../../components/forms/Checkbox";
import Input from "../../components/forms/Input";
import Card from "../../components/ui/Card";
import { useEducation } from "../../hooks/useEducation";
import { useFieldArray, useForm, useWatch } from "react-hook-form";
import Message from "../../components/ui/feedback/Message";
import type { EducationForm } from "../../models/forms/EducationForm";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import {
  createEducation,
  deleteEducation,
  updateEducation,
} from "../../api/educationApi";
import { useAuth0 } from "@auth0/auth0-react";
import Button from "../../components/ui/buttons/Button";

type EducationTextField =
  | "institutionName"
  | "qualification"
  | "fieldOfStudy"
  | "started"
  | "ended"
  | "useForResume";

const createListTag = (label: string, value?: string | null) => {
  let valueToDisplay = value ?? "Not specified";
  return (
    <li className="flex gap-2">
      <p className="font-semibold text-gray-500">{label}:</p>
      <p className="font-semibold text-gray-700">{valueToDisplay}</p>
    </li>
  );
};

const LibraryEducation = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { data: education, isLoading, isError } = useEducation();

  const [isSavedSuccessfully, setIsSavedSuccessfully] = useState<
    boolean | null
  >(null);

  const [editingEducationIndexes, setEditingEducationIndexes] = useState<
    Set<number>
  >(new Set());

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors, isValid },
  } = useForm<EducationForm>({
    mode: "onChange",
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
        institutionName: item.institutionName,
        qualification: item.qualification,
        fieldOfStudy: item.fieldOfStudy,
        started: item.started,
        ended: item.ended,
        useForResume: item.useForResume,
      })),
    });
  }, [education, reset]);

  const watchEducation = useWatch({
    control,
    name: "education",
  });

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
      id: null,
      institutionName: "",
      qualification: "",
      fieldOfStudy: "",
      gpa: null,
      started: "",
      ended: "",
      useForResume: false,
    });
    var newIndex = educationFields.length;
    setEditingEducationIndexes((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(newIndex)) {
        newSet.delete(newIndex);
      } else {
        newSet.add(newIndex);
      }
      return newSet;
    });
  };

  const toggleEducationEditMode = (index: number) => {
    setEditingEducationIndexes((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(index)) {
        newSet.delete(index);
      } else {
        newSet.add(index);
      }
      return newSet;
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

  function createInputOrView(
    label: string,
    field: EducationTextField,
    index: number,
    isRequired = false,
  ) {
    if (editingEducationIndexes.has(index)) {
      return (
        <Input
          id={`education.${index}.${field}`}
          label={`${label}${isRequired ? "*" : ""}`}
          registration={register(`education.${index}.${field}`, {
            required: isRequired ? "This field is required." : false,
          })}
          error={errors.education?.[index]?.[field]?.message}
        />
      );
    }

    return createListTag(label, watchEducation[index]?.[field]?.toString());
  }

  function createCheckboxOrView(
    label: string,
    field: EducationTextField,
    index: number,
    isRequired = false,
  ) {
    if (editingEducationIndexes.has(index)) {
      return (
        <Checkbox
          id={`education.${index}.${field}`}
          label={label}
          registration={register(`education.${index}.${field}`, {
            required: isRequired ? "This field is required." : false,
          })}
        />
      );
    }

    return createListTag(label, watchEducation[index]?.[field]?.toString());
  }

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Education Library</h1>
      <p className=" text-gray-600">
        Add and manage your educational background. You can include multiple
        entries, including degrees, certifications, and other relevant
        educational experiences.
      </p>
      <Card className="w-xl max-[640px]:w-full">
        <div>
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
                className={`flex flex-col border border-gray-300 p-4 rounded-md ${editingEducationIndexes.has(index) ? "gap-4" : "gap-1"}`}
              >
                <div className="flex justify-between items-center">
                  <h4 className="text-lg font-semibold">
                    Education {educationFields.length > 1 ? index + 1 : ""}
                  </h4>
                  <div className="flex gap-2">
                    <Button
                      label="Remove"
                      styleType="danger"
                      iconComponent={Trash3}
                      labelSlideOut={true}
                      onClick={() => handleRemoveEducation(index)}
                    />
                    <Button
                      label="Edit"
                      styleType="plain"
                      iconComponent={PencilSquare}
                      labelSlideOut={true}
                      onClick={() => toggleEducationEditMode(index)}
                    />
                  </div>
                </div>
                {createInputOrView(
                  "School Name",
                  "institutionName",
                  index,
                  true,
                )}
                {createInputOrView(
                  "Qualification",
                  "qualification",
                  index,
                  true,
                )}
                {createInputOrView("Field of Study", "fieldOfStudy", index)}
                {createInputOrView("Started", "started", index, true)}
                {createInputOrView("Ended", "ended", index)}
                {createCheckboxOrView(
                  "Use For Resume",
                  "useForResume",
                  index,
                  true,
                )}
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
        </div>
      </Card>
    </div>
  );
};

export default LibraryEducation;
