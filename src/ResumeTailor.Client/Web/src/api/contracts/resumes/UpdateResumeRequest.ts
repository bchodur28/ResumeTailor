import type { ResumeAppearanceRequest } from "./ResumeAppearanceRequest";
import type { ResumeExperienceRequest } from "./ResumeExperienceRequest";
import type { ResumeEducationRequest } from "./ResumeEducationRequest";

export type UpdateResumeRequest = {
  name: string;
  education: ResumeEducationRequest[];
  experiences: ResumeExperienceRequest[];
  appearance: ResumeAppearanceRequest;
};
