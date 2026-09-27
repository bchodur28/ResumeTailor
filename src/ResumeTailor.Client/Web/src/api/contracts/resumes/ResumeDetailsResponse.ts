import type { ResumeResponse } from "./ResumeResponse";
import type { ResumeAiAnalysisResponse } from "./ResumeAiAnalysisResponse";
import type { AiMetaDataResult } from "../../../models/ai/AiMetaDataResult";

export type ResumeDetailsResponse = {
  resume: ResumeResponse;
  aiAnalysis: ResumeAiAnalysisResponse;
  aiMetaData: AiMetaDataResult;
};
