import { useMutation } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { Controller, useForm } from "react-hook-form";
import { useAuth0 } from "@auth0/auth0-react";

import Card from "../components/ui/Card";
import Button from "../components/ui/buttons/Button";
import TextArea from "../components/forms/TextArea";
import Select from "../components/forms/Select";

import { useAccount } from "../contexts/AccountContext";
import { generateResume } from "../api/resumeApi";

import type { GenerateResumeRequest } from "../api/contracts/GenerateResumeRequest";
import type { GenerateResumeForm } from "../models/forms/GeneratedResumeForm";

import {
  SummaryGenerationMode,
  SummaryGenerationModeOptions,
} from "../models/resumes/SummaryGenerationMode";

const Generate = () => {
  const { account, isLoadingAccount, refreshAccount } = useAccount();
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  const {
    register,
    control,
    handleSubmit,
    formState: { errors, isValid },
  } = useForm<GenerateResumeForm>({
    mode: "onChange",
    defaultValues: {
      jobDescription: "",
      summaryMode: SummaryGenerationMode.None,
    },
  });

  const generateResumeMutation = useMutation({
    mutationFn: async ({
      accountId,
      request,
    }: {
      accountId: number;
      request: GenerateResumeRequest;
    }) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });

      return generateResume(accountId, request, token);
    },

    onSuccess: async (resumeId) => {
      await refreshAccount();
      navigate(`/app/resumes/${resumeId}`);
    },
  });

  const onSubmit = (data: GenerateResumeForm) => {
    if (!account) return;

    generateResumeMutation.mutate({
      accountId: account.id,
      request: {
        ...data,
        requestId: crypto.randomUUID(),
      },
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
        <div className="flex justify-between items-end">
          <h1 className="text-2xl font-semibold">Generate your Resume</h1>
          <h4 className="text-gray-600 font-semibold">
            Available Credits: {account.creditWallet?.availableCredits ?? 0}
          </h4>
        </div>

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
