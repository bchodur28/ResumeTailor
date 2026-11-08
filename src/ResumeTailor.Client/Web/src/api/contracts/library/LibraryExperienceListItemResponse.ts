import type { ExperienceTypeValue } from "../../../models/resumes/ExperienceType";

export type LibraryExperienceListItemResponse = {
  id: number;
  type: ExperienceTypeValue;
  title: string;
  organization: string | null;
  bulletCount: number;
  summaryCount: number;
  resumeCount: number;
};
