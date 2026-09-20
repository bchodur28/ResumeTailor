export type EducationResponse = {
  id: number;
  schoolName: string;
  degree: string;
  major: string;
  started: string;
  ended: string | null;
  useForResume: boolean;
};
