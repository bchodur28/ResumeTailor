export type LibraryExperienceSummaryResponse = {
  id: number;
  type: string;
  title: string;
  organization: string;
  summaryCount: number;
  letAiChooseSummary: boolean;
  bulletCount: number;
  letAiChooseBullets: boolean;
  maxBulletsToChoose: number | null;
};
