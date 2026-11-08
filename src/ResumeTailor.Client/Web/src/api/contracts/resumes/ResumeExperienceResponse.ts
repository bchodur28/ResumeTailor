import type { ResumeExperienceSummaryResponse } from "./ResumeExperienceSummaryResponse";
import type { ResumeBulletResponse } from "./ResumeBulletResponse";
import type { Experience } from "../../../models/Experience/Experience";

export type ResumeExperienceResponse = Experience & {
  id: number | null;
  libraryExperienceId: number;
  primarySummary: ResumeExperienceSummaryResponse | null;
  sortOrder: number;
  bullets: ResumeBulletResponse[];
};
