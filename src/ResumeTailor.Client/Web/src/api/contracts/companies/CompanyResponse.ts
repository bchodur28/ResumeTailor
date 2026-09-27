export type CompanyResponse = {
  id: number;
  name: string;
  title: string;
  location: string;
  started: string;
  ended: string | null;
  generateBullets: boolean;
  maxGeneratedBulletCount: number;
  bulletCount: number;
};
