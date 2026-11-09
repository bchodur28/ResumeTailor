import type { Education } from "../../../models/education/Education";

export type LibraryEducationResponse = Education & {
  id: number;
  useForResume: boolean;
  resumeCount: number;
};
