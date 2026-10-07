import type { ResumeBulletResponse } from "./ResumeBulletResponse";

export type ResumeCompanyResponse = {
  companyId: number;
  selectionId: number;
  name: string;
  title: string;
  location: string;
  started: string;
  ended: string | null;
  bullets: ResumeBulletResponse[];
};
