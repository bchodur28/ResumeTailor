export type ResumeBulletRequest = {
  id: number | null;
  sourceBulletId: number | null;
  value: string;
  alternativeValue: string | null;
  sortOrder: number;
};
