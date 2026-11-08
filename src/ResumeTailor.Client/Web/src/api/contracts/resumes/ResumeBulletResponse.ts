export type ResumeBulletResponse = {
  id: number;
  libraryBulletId: number;
  value: string;
  aiAlternativeValue: string | null;
  aiReasoning: string | null;
  sortOrder: number;
  isSourceDeleted: boolean;
};
