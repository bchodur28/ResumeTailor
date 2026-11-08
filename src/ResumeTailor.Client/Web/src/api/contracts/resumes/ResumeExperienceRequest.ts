import type { ResumeBulletRequest } from "./ResumeBulletRequest";

export type ResumeExperienceRequest = {
  id: number | null;
  libraryExperienceId: number;
  librarySummaryId: number | null;
  sortOrder: number;
  bullets: ResumeBulletRequest[];
};
