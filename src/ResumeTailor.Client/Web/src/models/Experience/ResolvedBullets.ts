export type ResolvedBullet = {
  resumeBulletId: number | null;
  libraryBulletId: number;
  value: string;
  aiAlternativeValue: string | null;
  aiReasoning: string | null;
  sortOrder: number;
  isSourceDeleted: boolean;
  useInResume: boolean;
};
