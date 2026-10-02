import type { ResumeBulletRequest } from "../bullets/ResumeBulletRequest";

export type ResumeCompanySelectionRequest = {
  id: number | null;
  resumeId: number;
  companyId: number;
  sortOrder: number;
  bullets: ResumeBulletRequest[];
};
