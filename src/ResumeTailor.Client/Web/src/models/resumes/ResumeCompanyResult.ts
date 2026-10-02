import type { ResumeBulletResult } from "./ResumeBulletResult";

export type ResumeCompanyResult = {
  companyId: number;
  selectionId: number | null;
  name: string;
  title: string;
  location: string | null;
  started: string;
  ended: string | null;
  bullets: ResumeBulletResult[];
};
