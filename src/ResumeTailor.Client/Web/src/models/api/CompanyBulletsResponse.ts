import type { BulletResponse } from "./BulletResponse";

export type CompanyBulletsResponse = {
  companyId: number;
  companyName: string;
  bullets: BulletResponse[];
};
