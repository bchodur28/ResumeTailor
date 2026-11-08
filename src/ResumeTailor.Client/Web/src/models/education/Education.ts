import type { EducationTypeValue } from "./EducationType";

export type Education = {
  type: EducationTypeValue;
  institutionName: string;
  qualification: string;
  fieldOfStudy: string | null;
  gpa: number | null;
  location: string | null;
  started: string;
  ended: string | null;
};
