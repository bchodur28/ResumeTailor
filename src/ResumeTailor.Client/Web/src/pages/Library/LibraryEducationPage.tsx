import { Pen, PencilSquare, Trash3 } from "react-bootstrap-icons";
import type { EducationRequest } from "../../api/contracts/education/EducationRequest";
import {
  EducationType,
  getEducationTypeLabel,
} from "../../models/education/EducationType";
import Checkbox from "../../components/forms/Checkbox";
import Input from "../../components/forms/Input";
import Card from "../../components/ui/Card";
import { useLibraryEducation } from "../../hooks/useLibraryEducation";
import { useFieldArray, useForm, useWatch } from "react-hook-form";
import type { EducationTypeValue } from "../../models/education/EducationType";
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
import InputView from "./components/InputView";
import CheckboxView from "./components/CheckboxView";
import DropdownButton from "../../components/ui/buttons/DropdownButton";
import ActionList from "../../components/ui/lists/ActionList";
import { getEducationFieldConfig } from "./EducationConfiguration";

const LibraryEducation = () => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { data: education, isLoading, isError } = useLibraryEducation();

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
    formState: { errors, isValid, isDirty, touchedFields, isSubmitted },
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
        type: item.type,
        institutionName: item.institutionName,
        qualification: item.qualification,
        fieldOfStudy: item.fieldOfStudy,
        gpa: item.gpa,
        location: item.location,
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

  console.log("isSubmitted:", isSubmitted);
  console.log("Errors:", errors);

  console.log("API Education:", education);
  console.log("Watched Education:", watchEducation);
  console.log("Education Fields:", educationFields);

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

  const handleAddEducation = (educationType: EducationTypeValue) => {
    const newIndex = educationFields.length;

    appendEducaiton({
      id: null,
      type: educationType,
      institutionName: "",
      qualification: "",
      fieldOfStudy: "",
      gpa: null,
      started: "",
      ended: "",
      useForResume: false,
    });

    setEditingEducationIndexes((prev) => {
      const newSet = new Set(prev);
      newSet.add(newIndex);
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

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Education Library</h1>
      <p className=" text-gray-600">
        Add and manage your educational background. You can include multiple
        entries, including degrees, certifications, and other relevant
        educational experiences.
      </p>
      <Card className="w-xl max-[640px]:w-full flex flex-col gap-4">
        <div>
          <form
            className="flex flex-col gap-4"
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
            {educationFields.map((field, index) => {
              const currentEducation = watchEducation?.[index];
              const config = getEducationFieldConfig(currentEducation?.type);
              const isEditing = editingEducationIndexes.has(index);
              return (
                <div
                  key={field.id}
                  className={`flex flex-col border primary-border p-4 rounded-md ${editingEducationIndexes.has(index) ? "gap-4" : "gap-0.5"}`}
                >
                  <div className="flex justify-between items-center">
                    <h4 className="text-xl font-medium primary-color-dark">
                      {currentEducation?.type
                        ? getEducationTypeLabel(currentEducation.type)
                        : "Education"}{" "}
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
                        iconComponent={Pen}
                        labelSlideOut={true}
                        onClick={() => toggleEducationEditMode(index)}
                        hideLabelOnMobile={true}
                      />
                    </div>
                  </div>

                  <InputView
                    id={`education.${index}.institutionName`}
                    label={config.institutionLabel}
                    isInput={isEditing}
                    valueToDisplay={currentEducation?.institutionName}
                    registration={register(
                      `education.${index}.institutionName`,
                      {
                        required: "This field is required.",
                      },
                    )}
                    error={
                      touchedFields.education?.[index]?.institutionName ||
                      isSubmitted
                        ? errors.education?.[index]?.institutionName?.message
                        : undefined
                    }
                    isValid={!errors.education?.[index]?.institutionName}
                  />

                  <InputView
                    id={`education.${index}.qualification`}
                    label={config.qualificationLabel}
                    isInput={isEditing}
                    valueToDisplay={currentEducation?.qualification}
                    registration={register(`education.${index}.qualification`, {
                      required: "This field is required.",
                    })}
                    error={
                      touchedFields.education?.[index]?.qualification ||
                      isSubmitted
                        ? errors.education?.[index]?.qualification?.message
                        : undefined
                    }
                    isValid={!errors.education?.[index]?.qualification}
                  />

                  {config.showFieldOfStudy && (
                    <InputView
                      id={`education.${index}.fieldOfStudy`}
                      label="Field of Study"
                      isInput={isEditing}
                      valueToDisplay={currentEducation?.fieldOfStudy ?? ""}
                      registration={register(`education.${index}.fieldOfStudy`)}
                      error={errors.education?.[index]?.fieldOfStudy?.message}
                    />
                  )}

                  {config.showStarted && (
                    <InputView
                      id={`education.${index}.started`}
                      label={config.startedLabel}
                      isInput={isEditing}
                      type="date"
                      valueToDisplay={currentEducation?.started}
                      registration={register(`education.${index}.started`)}
                    />
                  )}

                  <InputView
                    id={`education.${index}.ended`}
                    label={config.endedLabel}
                    isInput={isEditing}
                    type="date"
                    valueToDisplay={currentEducation?.ended ?? ""}
                    registration={register(`education.${index}.ended`)}
                  />
                  <CheckboxView
                    id="useForResume"
                    label="Use For Resume"
                    isInput={editingEducationIndexes.has(index)}
                    valueToDisplay={
                      currentEducation?.useForResume ? "Yes" : "No"
                    }
                    registration={register(`education.${index}.useForResume`)}
                  />
                </div>
              );
            })}
            {educationFields.length === 0 && (
              <div className="flex justify-center rounded-lg bg-gray-200 border border-gray-300 p-4 shadow-md">
                <p className="text-gray-700 font-semibold">
                  No education entries. Click to add.
                </p>
              </div>
            )}
            <div className="flex flex-col gap-4">
              {isDirty && (
                <Message
                  type="note"
                  message="You have unsaved changes. Save to apply your updates."
                />
              )}
              <div className="flex gap-2">
                <div className="flex-1 min-w-0">
                  <DropdownButton
                    label="Add"
                    styleType="plain"
                    closeOnClick={true}
                    btnClassName="w-full"
                    positionRelativeTo="container"
                    fullWidth={true}
                  >
                    <ActionList
                      actions={Object.values(EducationType).map((value) => ({
                        actionName: getEducationTypeLabel(value),
                        actionFn: () => handleAddEducation(value),
                      }))}
                    />
                  </DropdownButton>
                </div>

                {isDirty && (
                  <div className="flex-1 min-w-0">
                    <Button
                      label="Save Changes"
                      type="submit"
                      btnClassName="w-full"
                    />
                  </div>
                )}
              </div>
            </div>
          </form>
        </div>
      </Card>
    </div>
  );
};

export default LibraryEducation;
