import type { BulletResponse } from "../bullets/BulletResponse";

export type CompanyBulletsResponse = {
  companyId: number;
  companyName: string;
  bullets: BulletResponse[];
};
