export type ResumeBulletResponse = {
  id: number;
  sourceBulletId: number;
  value: string;
  alternativeValue: string | null;
  sortOrder: number | null;
  isSourceDeleted: boolean;
};
