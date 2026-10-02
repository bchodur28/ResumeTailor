export type EducationRequest = {
  id: number | null;
  schoolName: string;
  degree: string;
  major: string;
  started: string;
  ended: string | null;
  useForResume: boolean;
};
