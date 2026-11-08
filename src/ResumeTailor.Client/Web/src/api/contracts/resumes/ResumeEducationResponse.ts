import type { Education } from "../../../models/education/Education";

export type ResumeEducationResponse = Education & {
  id: number | null;
  libraryEducationId: number;
  sortOrder: number;
};
