import type { ResumeResponse } from "../../api/contracts/resumes/ResumeResponse";
import type { ResumeCompanyResponse } from "../../api/contracts/resumes/ResumeCompanyResponse";
import type { ResumeBulletResponse } from "../../api/contracts/resumes/ResumeBulletResponse";

export type ResumeBulletViewModel = Omit<ResumeBulletResponse, "id"> & {
  id: number | null;
};

export type ResumeCompanyViewModel = Omit<ResumeCompanyResponse, "bullets"> & {
  bullets: ResumeBulletViewModel[];
};

export type ResumeViewModel = Omit<ResumeResponse, "companies"> & {
  companies: ResumeCompanyViewModel[];
};
