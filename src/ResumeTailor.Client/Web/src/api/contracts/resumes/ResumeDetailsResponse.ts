import type { ResumeResponse } from "./ResumeResponse";
import type { ResumeAiAnalysisResponse } from "./ResumeAiAnalysisResponse";
import type { ApplicationTrackingResponse } from "./ApplicationTrackingResponse";
import type { AiMetaDataResult } from "../../../models/ai/AiMetaDataResult";
import type { JobPostingResult } from "../../../models/resumes/JobPostingResult";

export type ResumeDetailsResponse = {
  resume: ResumeResponse;
  aiAnalysis: ResumeAiAnalysisResponse | null;
  jobPosting: JobPostingResult;
  applicationTracking: ApplicationTrackingResponse;
  aiMetaData: AiMetaDataResult;
};
