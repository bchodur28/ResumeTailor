import type { SummaryGenerationModeValue } from "../../models/resumes/SummaryGenerationMode";

export type GenerateResumeRequest = {
  jobDescription: string;
  summaryMode: SummaryGenerationModeValue;
};
