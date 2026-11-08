import type { Education } from "./Education";

export type ResolvedEducation = Education & {
  resumeEducationId: number | null;
  libraryEducationId: number;
  sortOrder: number;
  useInResume: boolean;
};
