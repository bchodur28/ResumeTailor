import type { CompanyGeneratedResume } from "./CompanyGeneratedResume";
import type { AiUsage } from "../ai/AiUsage";
import type { AiSummary } from "../ai/AiSummary";

export type ResumeGenerationResult = {
  aiResume: CompanyGeneratedResume;
  aiSummary: AiSummary;
  aiUsage?: AiUsage;
};
