export type Bullet = {
  resumeBulletId: number | null;
  libraryBulletId: number;
  wasManuallyChanged: boolean;
  aiReasoning: string | null;
  sortOrder: number;
  value: string;
};
