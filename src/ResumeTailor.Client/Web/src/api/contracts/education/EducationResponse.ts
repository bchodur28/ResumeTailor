export type EducationResponse = {
  id: number;
  selectionId: number | null;
  schoolName: string;
  degree: string;
  major: string;
  started: string;
  ended: string | null;
  useForResume: boolean;
};
