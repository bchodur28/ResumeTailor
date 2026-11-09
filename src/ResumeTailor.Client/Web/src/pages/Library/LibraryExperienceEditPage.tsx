import type { UseFormRegisterReturn } from "react-hook-form";
import BulletEditor from "./components/BulletEditor";
import { useEffect, useState } from "react";
import Card from "../../components/ui/Card";
import { useLibraryExperience } from "../../hooks/useLibraryExperience";
import {
  Controller,
  FormProvider,
  get,
  useFieldArray,
  useForm,
} from "react-hook-form";
import {
  ExperienceType,
  getExperienceTypeLabel,
  type ExperienceTypeValue,
} from "../../models/resumes/ExperienceType";
import type { LibraryExperienceRequest } from "../../api/contracts/library/LibraryExperienceRequest";
import type { LibraryExperienceResponse } from "../../api/contracts/library/LibraryExperienceResponse";
import type { LibraryBulletResponse } from "../../api/contracts/library/LibraryBulletResponse";
import type { LibraryBulletRequest } from "../../api/contracts/library/LibraryBulletRequest";
import type { LibrarySummaryResponse } from "../../api/contracts/library/LibrarySummaryResponse";
import type { LibrarySummaryRequest } from "../../api/contracts/library/LibrarySummaryRequest";
import Input from "../../components/forms/Input";
import Select from "../../components/forms/Select";
import {
  PlusCircle,
  ChevronDown,
  ChevronUp,
  Trash3,
  ArrowCounterclockwise,
  Floppy,
  ExclamationCircle,
  CheckCircle,
  Briefcase,
  Calendar2,
  Calendar2X,
  Eyeglasses,
  Pen,
} from "react-bootstrap-icons";
import TextArea from "../../components/forms/TextArea";
import Button from "../../components/ui/buttons/Button";
import Checkbox from "../../components/forms/Checkbox";
import { useMutation } from "@tanstack/react-query";
import Message from "../../components/ui/feedback/Message";
import { useParams } from "react-router-dom";
import SummaryEditor from "./components/SummaryEditor";
import CollapsibleSection from "./components/CollapsibleSection";
import ControllerView from "./components/ControllerView";
import InputView from "./components/InputView";
import CheckboxView from "./components/CheckboxView";
import TextAreaView from "./components/TextAreaView";
import { useDateFormat } from "../../hooks/useDateFormat";

const mapExperienceResponseToRequest = (
  experience: LibraryExperienceResponse,
): LibraryExperienceRequest => {
  return {
    type: experience.type,
    title: experience.title,
    organization: experience.organization ?? null,
    location: experience.location ?? null,
    started: experience.started,
    ended: experience.ended ?? null,
    link: experience.link ?? null,
    techStack: experience.techStack ?? null,
    letAiChooseBullets: experience.letAiChooseBullets ?? true,
    maxBulletsToChoose: experience.maxBulletsToChoose,
    letAiChooseSummary: experience.letAiChooseSummary ?? true,
    summaries:
      experience.summaries.map((s) =>
        mapSummaryResponseToRequest(experience.id, s),
      ) ?? [],
    bullets:
      experience.bullets.map((b) =>
        mapBulletResponseToRequest(experience.id, b),
      ) ?? [],
  };
};

const mapBulletResponseToRequest = (
  experienceId: number,
  bullet: LibraryBulletResponse,
): LibraryBulletRequest => {
  return {
    id: bullet.id,
    experienceId: experienceId,
    value: bullet.value,
    isDeleting: bullet.isDeleted,
  };
};

const mapSummaryResponseToRequest = (
  experienceId: number,
  summary: LibrarySummaryResponse,
): LibrarySummaryRequest => {
  return {
    id: summary.id,
    experienceId: experienceId,
    value: summary.value,
    isPrimary: summary.isPrimary,
    isDeleting: summary.isDeleted,
  };
};

const createEmptyBullet = (experienceId: number): LibraryBulletRequest => ({
  id: null,
  experienceId: experienceId,
  value: "",
  isDeleting: false,
});

const createEmptySummary = (experienceId: number): LibrarySummaryRequest => ({
  id: null,
  experienceId: experienceId,
  value: "",
  isPrimary: false,
  isDeleting: false,
});

const getValidSwitchTypes = (
  experienceType: ExperienceTypeValue,
): ExperienceTypeValue[] => {
  switch (experienceType) {
    case ExperienceType.Employment:
      return [ExperienceType.Contract, ExperienceType.Freelance];
    case ExperienceType.Freelance:
      return [ExperienceType.Employment, ExperienceType.Internship];
    case ExperienceType.Internship:
      return [ExperienceType.Employment, ExperienceType.Contract];
    case ExperienceType.Contract:
      return [ExperienceType.Employment, ExperienceType.Internship];
    case ExperienceType.PersonalProject:
      return [
        ExperienceType.AcademicProject,
        ExperienceType.OpenSourceProject,
        ExperienceType.Research,
      ];
    case ExperienceType.AcademicProject:
      return [
        ExperienceType.PersonalProject,
        ExperienceType.OpenSourceProject,
        ExperienceType.Research,
      ];
    case ExperienceType.OpenSourceProject:
    case ExperienceType.Research:
      return [
        ExperienceType.PersonalProject,
        ExperienceType.AcademicProject,
        ExperienceType.OpenSourceProject,
        ExperienceType.Research,
      ];
    default:
      return [];
  }
};

const LibraryExperienceEditPage = () => {
  const { experienceId } = useParams();
  const id = Number(experienceId);

  const { formatDate } = useDateFormat();

  const { data: experience, isLoading, error } = useLibraryExperience(id);

  const [isEditing, setIsEditing] = useState(false);

  const [allowSummaryByDefault, setAllowSummaryByDefault] = useState(false);
  const [hideEndDate, setHideEndDate] = useState(false);
  const [successfulSave, setSuccessfulSave] = useState<boolean | null>(null);

  const methods = useForm<LibraryExperienceRequest>({
    mode: "onChange",
  });

  const {
    control,
    register,
    setValue,
    getValues,
    watch,
    handleSubmit,
    reset,
    resetField,
    formState: { errors, isValid, isDirty, dirtyFields },
  } = methods;

  const experienceType = watch("type");

  const discouragedSummary = [
    ExperienceType.Employment,
    ExperienceType.Internship,
    ExperienceType.Contract,
  ].some((type) => type === experienceType);

  useEffect(() => {
    if (experience) {
      reset(mapExperienceResponseToRequest(experience));

      const recommendedSummary = [
        ExperienceType.PersonalProject,
        ExperienceType.AcademicProject,
        ExperienceType.OpenSourceProject,
        ExperienceType.Research,
      ].some((type) => type === experience.type);

      setAllowSummaryByDefault(recommendedSummary);

      if (experience.ended === null) {
        setHideEndDate(true);
      }
    }
  }, [experience, reset]);

  const handleCancelExperienceDetails = () => {
    resetField("type");
    resetField("title");
    resetField("organization");
    resetField("location");
    resetField("started");
    resetField("ended");
    resetField("link");
    resetField("techStack");
    resetField("letAiChooseBullets");
    resetField("maxBulletsToChoose");

    setHideEndDate(experience?.ended == null);
    setIsEditing(false);
  };

  const experienceTypeOptions = experience
    ? [experience.type, ...getValidSwitchTypes(experience.type)].map(
        (value) => ({
          id: value,
          label: getExperienceTypeLabel(value),
        }),
      )
    : [];

  const {
    fields: summaryFields,
    append: appendSummary,
    remove: removeSummary,
  } = useFieldArray({
    control,
    name: "summaries",
  });

  const {
    fields: bulletFields,
    append: appendBullet,
    remove: removeBullet,
  } = useFieldArray({
    control,
    name: "bullets",
  });

  const flagBulletForDeletion = (index: number) => {
    const bullet = getValues(`bullets.${index}`);

    if (bullet?.id == null) {
      removeBullet(index);
      return;
    }

    setValue(`bullets.${index}.isDeleting`, true, {
      shouldDirty: true,
    });
  };

  const flagSummaryForDeletion = (index: number) => {
    const summary = getValues(`summaries.${index}`);

    if (summary?.id == null) {
      removeSummary(index);
      if (summaryFields.length === 1) {
        setAllowSummaryByDefault(false);
      }
      return;
    }

    setValue(`summaries.${index}.isDeleting`, true, {
      shouldDirty: true,
    });
  };

  const editLibraryExperienceMutation = useMutation({
    mutationFn: async (request: LibraryExperienceRequest) => {
      console.log("REQUEST", request);
    },
    onError: (error: any) => {
      console.error("Error editing library experience:", error);
      setSuccessfulSave(false);
    },
    onSuccess: () => {
      console.log("Library experience edited successfully");
      setSuccessfulSave(true);
    },
  });

  const onSubmit = (data: LibraryExperienceRequest) => {
    if (!data.letAiChooseBullets) {
      data.maxBulletsToChoose = null;
    }
    editLibraryExperienceMutation.mutate(data);
  };

  return (
    <div className="flex flex-col gap-4 items-center w-xl max-[640px]:w-full">
      <h1 className="text-2xl font-bold">Edit Experience Library</h1>
      <p className=" text-gray-600">
        Edit your experience details below. Add multiple bullets and summaries
        to accurately describe your experience. AI will use this information to
        generate your resume per posting description. You control how many
        bullets are included in resumes.
      </p>
      <Card className="w-xl max-[640px]:w-full flex flex-col gap-4">
        {isDirty && (
          <Message
            type="note"
            message="You have unsaved changes. Save to apply your updates."
          />
        )}
        {isLoading && <p>Loading...</p>}
        {error && <p className="text-red-500">{error.message}</p>}
        {successfulSave && (
          <Message
            type="success"
            message="Changes saved successfully"
            onClose={() => setSuccessfulSave(null)}
          />
        )}
        {successfulSave === false && (
          <Message
            type="error"
            message="Failed to save changes"
            onClose={() => setSuccessfulSave(null)}
          />
        )}
        {experience && (
          <FormProvider {...methods}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              className="flex flex-col gap-4"
            >
              <CollapsibleSection
                title="Experience Details"
                openByDefault={true}
                actions={[
                  {
                    label: isEditing ? "Cancel" : "Edit",
                    iconComponent: isEditing ? ArrowCounterclockwise : Pen,
                    onClick: () => {
                      if (isEditing) {
                        handleCancelExperienceDetails();
                      } else {
                        setIsEditing(true);
                      }
                    },
                    expandOnClick: true,
                    hideLabelOnMobile: true,
                  },
                ]}
              >
                <div
                  className={`flex flex-col ${isEditing ? "gap-4" : "gap-0.5"} rounded-lg`}
                >
                  <ControllerView
                    name="type"
                    label="Experience Type"
                    options={experienceTypeOptions}
                    control={control}
                    isInput={isEditing}
                    isRequired={true}
                  />
                  {/* <Input
                    id="title"
                    label="Title"
                    registration={register("title", {
                      required: "Title is required",
                    })}
                    error={errors.title?.message}
                    isValid={!errors.title}
                  /> */}
                  <InputView
                    id="title"
                    label="Title"
                    isInput={isEditing}
                    valueToDisplay={watch("title")}
                    registration={register("title", {
                      required: "Title is required",
                    })}
                    error={errors.title?.message}
                    isValid={!errors.title}
                  />
                  {watch("type") === ExperienceType.Employment && (
                    <>
                      <InputView
                        id="organization"
                        label="Organization"
                        isInput={isEditing}
                        valueToDisplay={watch("organization")}
                        registration={register("organization", {
                          required: "Organization is required",
                        })}
                        error={errors.organization?.message}
                        isValid={!errors.organization}
                      />
                      <InputView
                        id="location"
                        label="Location"
                        isInput={isEditing}
                        valueToDisplay={watch("location")}
                        registration={register("location", {
                          required: "Location is required",
                        })}
                        error={errors.location?.message}
                        isValid={!errors.location}
                      />
                    </>
                  )}

                  <InputView
                    id="start-date"
                    label="Start Date"
                    isInput={isEditing}
                    type="date"
                    valueToDisplay={watch("started")}
                    registration={register("started", {
                      required: "Start Date is required",
                    })}
                    error={errors.started?.message}
                    isValid={!errors.started}
                    dateFormatterFn={formatDate}
                  />
                  <div className="flex items-end gap-2 justify-stretch">
                    {!hideEndDate && (
                      <InputView
                        id="end-date"
                        label="End Date"
                        isInput={isEditing}
                        type="date"
                        valueToDisplay={watch("ended") ?? "Currently Working"}
                        registration={register(
                          "ended",
                          hideEndDate
                            ? {}
                            : {
                                required: "End Date is required",
                              },
                        )}
                        error={errors.ended?.message}
                        isValid={!errors.ended}
                        dateFormatterFn={formatDate}
                      />
                    )}
                    {isEditing && (
                      <Button
                        label={
                          hideEndDate ? "Add End Date" : "Currently Working"
                        }
                        onClick={() => setHideEndDate(!hideEndDate)}
                        styleType="plain"
                        iconComponent={hideEndDate ? Calendar2X : Briefcase}
                        labelSlideOut={true}
                      />
                    )}
                  </div>

                  {watch("type") === ExperienceType.PersonalProject && (
                    <>
                      <TextAreaView
                        id="tech-stack"
                        label="Tech Stack"
                        isInput={isEditing}
                        valueToDisplay={watch("techStack")}
                        registration={register("techStack")}
                        rows={4}
                      />
                      <InputView
                        id="link"
                        label="Link"
                        isInput={isEditing}
                        valueToDisplay={watch("link")}
                        registration={register("link")}
                      />
                    </>
                  )}
                  {(watch("type") === ExperienceType.Employment ||
                    watch("type") === ExperienceType.Internship ||
                    watch("type") === ExperienceType.Freelance ||
                    watch("type") === ExperienceType.Contract) && (
                    <>
                      {/* <Checkbox
                        id="letAiChooseBullets"
                        label="Let AI Choose Bullets"
                        registration={register("letAiChooseBullets")}
                      /> */}
                      <CheckboxView
                        id="letAiChooseBullets"
                        label="Let AI Choose Bullets"
                        isInput={isEditing}
                        valueToDisplay={
                          watch("letAiChooseBullets") ? "Yes" : "No"
                        }
                        registration={register("letAiChooseBullets")}
                      />

                      {watch("letAiChooseBullets") && (
                        <InputView
                          id="maxBulletsToChoose"
                          label="Max Bullets to Choose"
                          isInput={isEditing}
                          type="number"
                          valueToDisplay={watch(
                            "maxBulletsToChoose",
                          )?.toString()}
                          registration={register("maxBulletsToChoose", {
                            required:
                              "Value is required and must be greater than 0",
                            validate: (value) =>
                              (value && value > 0) ||
                              "Value must be greater than 0",
                          })}
                        />
                      )}
                    </>
                  )}
                </div>
              </CollapsibleSection>

              {!allowSummaryByDefault && (
                <Button
                  label="Add Summaries"
                  onClick={() => {
                    appendSummary(createEmptySummary(experience.id));
                    setAllowSummaryByDefault(true);
                  }}
                  styleType="plain"
                  optionalWarning={
                    discouragedSummary
                      ? `Summaries are generally unnecessary for ${watch("type")} experience. Bullet points are usually sufficient.`
                      : undefined
                  }
                />
              )}
              {summaryFields.length > 0 && allowSummaryByDefault && (
                <CollapsibleSection
                  title="Summaries"
                  optionalMessage={`(${summaryFields.length} out of 3)`}
                  openByDefault={true}
                  actions={
                    summaryFields.length > 2
                      ? undefined
                      : [
                          {
                            label: "Add",
                            iconComponent: PlusCircle,
                            onClick: () => {
                              appendSummary(createEmptySummary(experience.id));
                            },
                            expandOnClick: true,
                          },
                        ]
                  }
                >
                  <div className="flex flex-col gap-4">
                    {summaryFields.map((summary, index) => {
                      return (
                        <SummaryEditor
                          key={summary.id}
                          index={index}
                          onDelete={flagSummaryForDeletion}
                        />
                      );
                    })}
                  </div>
                </CollapsibleSection>
              )}
              {bulletFields.length > 0 && (
                <CollapsibleSection
                  title="Bullets"
                  optionalMessage={`(${bulletFields.length} bullets)`}
                  actions={[
                    {
                      label: "Add",
                      iconComponent: PlusCircle,
                      onClick: () => {
                        appendBullet(createEmptyBullet(experience.id));
                      },
                      expandOnClick: true,
                    },
                  ]}
                >
                  <div className="flex flex-col gap-4">
                    {bulletFields.map((bullet, index) => {
                      return (
                        <BulletEditor
                          key={bullet.id}
                          index={index}
                          onDelete={flagBulletForDeletion}
                        />
                      );
                    })}
                  </div>
                </CollapsibleSection>
              )}
              <div className="flex gap-2">
                {bulletFields.length === 0 && (
                  <Button
                    label="Add Bullets"
                    onClick={() => {
                      appendBullet(createEmptyBullet(experience.id));
                    }}
                    styleType="plain"
                    grow
                  />
                )}
                {isDirty && <Button label="Save Changes" type="submit" grow />}
              </div>
            </form>
          </FormProvider>
        )}
      </Card>
    </div>
  );
};

export default LibraryExperienceEditPage;
