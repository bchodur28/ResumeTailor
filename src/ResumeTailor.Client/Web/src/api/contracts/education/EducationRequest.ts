export type EducationRequest = {
  id: number | null;
  institutionName: string;
  qualification: string;
  fieldOfStudy: string | null;
  gpa: number | null;
  started: string;
  ended: string | null;
  useForResume: boolean;
};
