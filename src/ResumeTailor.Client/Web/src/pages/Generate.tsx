import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { Controller, useForm } from "react-hook-form";

import Card from "../components/ui/Card";
import Button from "../components/ui/buttons/Button";
import TextArea from "../components/forms/TextArea";
import Select from "../components/forms/Select";

import { useAccount } from "../contexts/AccountContext";
import { generateResume } from "../api/resumeApi";

import type { GenerateResumeRequest } from "../api/contracts/GenerateResumeRequest";

import {
  SummaryGenerationMode,
  SummaryGenerationModeOptions,
} from "../models/resumes/SummaryGenerationMode";

const Generate = () => {
  const { account, isLoadingAccount } = useAccount();
  const navigate = useNavigate();

  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isValid },
  } = useForm<GenerateResumeRequest>({
    mode: "onChange",
    defaultValues: {
      jobDescription: "",
      summaryMode: SummaryGenerationMode.None,
    },
  });

  const generateResumeMutation = useMutation({
    mutationFn: ({
      accountId,
      request,
    }: {
      accountId: number;
      request: GenerateResumeRequest;
    }) => generateResume(accountId, request),

    onSuccess: (resumeId) => {
      navigate(`/resumes/${resumeId}`);
    },
  });

  const onSubmit = (data: GenerateResumeRequest) => {
    if (!account) return;

    generateResumeMutation.mutate({
      accountId: account.id,
      request: data,
    });
  };

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  if (!account) {
    return <div>No Account</div>;
  }

  return (
    <div className="flex justify-center">
      <Card className="w-5xl flex flex-col gap-4">
        <h1 className="text-2xl font-semibold">Generate your Resume</h1>

        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
          <TextArea
            id="jobDescription"
            label="Job Description"
            registration={register("jobDescription", {
              required: "Job description is required",
            })}
            error={errors.jobDescription?.message}
            rows={25}
          />

          <Controller
            name="summaryMode"
            control={control}
            render={({ field }) => (
              <Select
                id="summaryMode"
                label="Summary Mode"
                options={SummaryGenerationModeOptions}
                value={SummaryGenerationModeOptions.find(
                  (option) => option.id === field.value,
                )}
                onChange={(option) => field.onChange(option?.id)}
              />
            )}
          />

          <Button
            label={
              generateResumeMutation.isPending
                ? "Generating resume..."
                : "Generate Resume"
            }
            type="submit"
            disabled={!isValid || generateResumeMutation.isPending}
          />
        </form>
      </Card>
    </div>
  );
};

export default Generate;
