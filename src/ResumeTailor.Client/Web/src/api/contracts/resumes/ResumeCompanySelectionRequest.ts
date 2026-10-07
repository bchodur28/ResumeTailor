import type { ResumeBulletRequest } from "./BulletSelectionRequest";

export type ResumeCompanySelectionRequest = {
  id: number | null;
  resumeId: number;
  companyId: number;
  sortOrder: number;
  bullets: ResumeBulletRequest[];
};
