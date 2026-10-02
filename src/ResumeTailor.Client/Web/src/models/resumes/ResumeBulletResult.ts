export type ResumeBulletResult = {
  id: number | null;
  selectionId: number | null;
  sourceBulletId: number | null;
  value: string;
  alternativeValue: string | null;
  sortOrder: number | null;
  isSourceDeleted: boolean;
};
