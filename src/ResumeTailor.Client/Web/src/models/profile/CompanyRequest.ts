export type CompanyRequest = {
  id: number | null;
  name: string;
  title: string;
  location: string;
  started: string;
  ended: string | null;
  generateBullets: boolean;
  maxGeneratedBulletCount: number;
};
