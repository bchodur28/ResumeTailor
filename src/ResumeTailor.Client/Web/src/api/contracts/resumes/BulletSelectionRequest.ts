export type ResumeBulletRequest = {
  id: number | null;
  bulletId: number;
  value: string;
  alternativeValue: string | null;
  sortOrder: number;
};
