export type LibrarySummaryResponse = {
  id: number;
  isPrimary: boolean;
  value: string;
  aiRewrite: string | null;
  aiOpinion: string | null;
  aiScore: number | null;
  isDeleted: boolean;
};
