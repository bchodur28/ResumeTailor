import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useDateFormat } from "../../../hooks/useDateFormat";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuth0 } from "@auth0/auth0-react";
import {
  Archive,
  ArrowCounterclockwise,
  Pen,
  Trash3,
} from "react-bootstrap-icons";
import type { LibraryEducationResponse } from "../../../api/contracts/education/LibraryEducationResponse";
import {
  getEducationTypeLabel,
  type EducationTypeValue,
} from "../../../models/education/EducationType";
import type { EducationRequest } from "../../../api/contracts/education/EducationRequest";
import { getEducationFieldConfig } from "../EducationConfiguration";
import {
  archiveLibraryEducation,
  createEducation,
  deleteEducation,
  restoreLibraryEducation,
  updateEducation,
} from "../../../api/educationApi";
import Button from "../../../components/ui/buttons/Button";
import Message from "../../../components/ui/feedback/Message";
import CheckboxView from "./CheckboxView";
import InputView from "./InputView";

type EducationAction = "archived" | "restored" | "deleted" | "saved";

type Props = (
  | {
      education: LibraryEducationResponse;
      archived: boolean;
      educationType?: never;
      onRemove?: never;
      onCreated?: never;
    }
  | {
      education?: never;
      educationType: EducationTypeValue;
      archived?: never;
      onRemove: () => void;
      onCreated: () => void;
    }
) & {
  onSuccess?: (action: EducationAction) => void;
};

const LibraryEducationItem = ({
  education,
  archived = false,
  educationType,
  onRemove,
  onCreated,
  onSuccess,
}: Props) => {
  const { getAccessTokenSilently } = useAuth0();
  const queryClient = useQueryClient();

  const { formatDate } = useDateFormat();

  const isNew = !education;
  const canDelete = education != null && education.resumeCount === 0;
  const [isEditing, setIsEditing] = useState(isNew);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    reset,
    watch,
    formState: {
      errors,
      isDirty,
      isValid,
      isSubmitting,
      touchedFields,
      isSubmitted,
    },
  } = useForm<EducationRequest>({
    mode: "onChange",
    defaultValues: {
      type: education?.type ?? educationType,
      institutionName: education?.institutionName ?? "",
      qualification: education?.qualification ?? "",
      fieldOfStudy: education?.fieldOfStudy ?? "",
      location: education?.location ?? null,
      gpa: education?.gpa ?? null,
      started: education?.started ?? "",
      ended: education?.ended ?? "",
      useForResume: education?.useForResume ?? false,
    },
  });

  const currentEducation = watch();
  const config = getEducationFieldConfig(currentEducation.type);

  useEffect(() => {
    if (education) {
      reset({
        type: education.type,
        institutionName: education.institutionName,
        qualification: education.qualification,
        fieldOfStudy: education.fieldOfStudy,
        location: education.location,
        gpa: education.gpa,
        started: education.started,
        ended: education.ended ?? "",
        useForResume: education.useForResume,
      });
    }
  }, [education, reset]);

  const saveMutation = useMutation({
    mutationFn: async (data: EducationRequest) => {
      const token = await getAccessTokenSilently();

      const request = {
        ...data,
        started: data.started || null,
        ended: data.ended || null,
      };

      if (isNew) {
        await createEducation(request, token);
      } else {
        await updateEducation(education.id, request, token);
      }
    },

    onSuccess: (_result, submittedData) => {
      setErrorMessage(null);

      if (isNew) {
        onCreated?.();
      } else {
        reset(submittedData);
        setIsEditing(false);
      }

      queryClient.invalidateQueries({
        queryKey: ["library-education"],
      });

      onSuccess?.("saved");
    },

    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async () => {
      if (!education) return;

      const token = await getAccessTokenSilently();
      await deleteEducation(education.id, token);
    },

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["library-education"],
      });
      onSuccess?.("deleted");
    },

    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  const archiveMutation = useMutation({
    mutationFn: async () => {
      if (!education) return;

      const token = await getAccessTokenSilently();
      await archiveLibraryEducation(education.id, token);
    },

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["library-education"],
      });
      onSuccess?.("archived");
    },

    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  const restoreMutation = useMutation({
    mutationFn: async () => {
      if (!education) return;

      const token = await getAccessTokenSilently();
      await restoreLibraryEducation(education.id, token);
    },

    onSuccess: () => {
      setErrorMessage(null);

      queryClient.invalidateQueries({
        queryKey: ["library-education"],
      });
      onSuccess?.("restored");
    },

    onError: (error: Error) => {
      setErrorMessage(error.message);
    },
  });

  const handleRemove = () => {
    if (isNew) {
      onRemove?.();
      return;
    }

    if (window.confirm("Permanently delete this education?")) {
      deleteMutation.mutate();
    }
  };

  const handleCancel = () => {
    if (isNew) {
      onRemove?.();
    } else {
      reset();
      setIsEditing(false);
    }
  };

  return (
    <form
      onSubmit={handleSubmit((data) => saveMutation.mutate(data))}
      className={`flex flex-col border primary-border p-4 rounded-md ${
        isEditing ? "gap-4" : "gap-2"
      }`}
    >
      <h4 className="text-xl font-medium primary-color-dark">
        {getEducationTypeLabel(currentEducation.type)}
      </h4>
      <div className={`flex flex-col ${isEditing ? "gap-4" : "gap-0.5"}`}>
        <InputView
          id="institutionName"
          label={config.institutionLabel}
          isInput={isEditing}
          valueToDisplay={currentEducation.institutionName}
          registration={register("institutionName", {
            required: "This field is required.",
          })}
          error={
            touchedFields.institutionName || isSubmitted
              ? errors.institutionName?.message
              : undefined
          }
          isValid={
            (touchedFields.institutionName || isSubmitted) &&
            !errors.institutionName
          }
        />

        <InputView
          id="qualification"
          label={config.qualificationLabel}
          isInput={isEditing}
          valueToDisplay={currentEducation.qualification}
          registration={register("qualification", {
            required: "This field is required.",
          })}
          error={
            touchedFields.qualification || isSubmitted
              ? errors.qualification?.message
              : undefined
          }
          isValid={
            (touchedFields.qualification || isSubmitted) &&
            !errors.qualification
          }
        />

        {config.showFieldOfStudy && (
          <InputView
            id="fieldOfStudy"
            label="Field of Study"
            isInput={isEditing}
            valueToDisplay={currentEducation.fieldOfStudy ?? ""}
            registration={register("fieldOfStudy")}
          />
        )}

        {config.showStarted && (
          <InputView
            id="started"
            label={config.startedLabel}
            isInput={isEditing}
            type="date"
            valueToDisplay={currentEducation.started}
            registration={register("started")}
            dateFormatterFn={formatDate}
          />
        )}

        <InputView
          id="ended"
          label={config.endedLabel}
          isInput={isEditing}
          type="date"
          valueToDisplay={currentEducation.ended ?? ""}
          registration={register("ended")}
          dateFormatterFn={formatDate}
        />

        <CheckboxView
          id="useForResume"
          label="Use For Resume"
          isInput={isEditing}
          valueToDisplay={currentEducation.useForResume ? "Yes" : "No"}
          registration={register("useForResume")}
        />

        {education && education.resumeCount > 0 && (
          <p className="text-sm text-gray-500">
            Used in {education.resumeCount}{" "}
            {education.resumeCount === 1 ? "resume" : "resumes"}.
            {!archived && " Archive to remove from your active library."}
          </p>
        )}
      </div>

      <div className="flex justify-end gap-2">
        {isNew ? (
          <Button
            label="Remove"
            styleType="danger"
            iconComponent={Trash3}
            onClick={handleRemove}
            labelSlideOut
          />
        ) : (
          <>
            {archived ? (
              <Button
                label="Restore"
                styleType="plain"
                iconComponent={ArrowCounterclockwise}
                onClick={() => restoreMutation.mutate()}
                labelSlideOut
              />
            ) : (
              <>
                <Button
                  label="Archive"
                  styleType="plain"
                  iconComponent={Archive}
                  onClick={() => archiveMutation.mutate()}
                  labelSlideOut
                />

                <Button
                  label={isEditing ? "Cancel" : "Edit"}
                  styleType="plain"
                  iconComponent={isEditing ? ArrowCounterclockwise : Pen}
                  onClick={() =>
                    isEditing ? handleCancel() : setIsEditing(true)
                  }
                  labelSlideOut
                />
              </>
            )}
            {canDelete && (
              <Button
                label="Delete"
                styleType="danger"
                iconComponent={Trash3}
                onClick={handleRemove}
                labelSlideOut
              />
            )}
          </>
        )}
      </div>

      {errorMessage && <Message type="error" message={errorMessage} />}

      {!archived && isEditing && (isNew || isDirty) && (
        <Button label={isNew ? "Create" : "Save Changes"} type="submit" grow />
      )}
    </form>
  );
};

export default LibraryEducationItem;
