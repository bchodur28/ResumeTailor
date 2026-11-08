import type { ResumeResponse } from "../../api/contracts/resumes/ResumeResponse";
import type { ResumeBulletResponse } from "../../api/contracts/resumes/ResumeBulletResponse";
import type { ResumeExperienceResponse } from "../../api/contracts/resumes/ResumeExperienceResponse";

export type ResumeBulletViewModel = Omit<ResumeBulletResponse, "id"> & {
  id: number | null;
};

export type ResumeExperienceViewModel = Omit<
  ResumeExperienceResponse,
  "bullets"
> & {
  bullets: ResumeBulletViewModel[];
};

export type ResumeViewModel = Omit<ResumeResponse, "experiences"> & {
  experiences: ResumeExperienceViewModel[];
};
