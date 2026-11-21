import type { JobPostingResult } from "../../../models/resumes/JobPostingResult";
import type { ApplicationTrackingResponse } from "./ApplicationTrackingResponse";

export type ResumeSummaryResponse = {
  id: number;
  accountId: number;
  name: string;
  aiScore: number | null;
  jobPosting: JobPostingResult | null;
  applicationTracking: ApplicationTrackingResponse | null;
};
