export type LibraryBulletResponse = {
  id: number;
  value: string;
  aiRewrite: string | null;
  aiOpinion: string | null;
  aiScore: number | null;
  isDeleted: boolean;
};
