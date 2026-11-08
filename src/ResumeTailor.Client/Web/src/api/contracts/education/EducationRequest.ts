import type { EducationTypeValue } from "../../../models/education/EducationType";

export type EducationRequest = {
  id: number | null;
  type: EducationTypeValue;
  institutionName: string;
  qualification: string;
  fieldOfStudy: string | null;
  gpa: number | null;
  started: string;
  ended: string | null;
  useForResume: boolean;
};
