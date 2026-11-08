import type { ResumeAppearanceResponse } from "./ResumeAppearanceResponse";
import type { PersonalLinkResponse } from "../accounts/PersonalLinkResponse";
import type { SkillResponse } from "../accounts/SkillResponse";
import type { ResumeExperienceResponse } from "./ResumeExperienceResponse";
import type { ResumeEducationResponse } from "./ResumeEducationResponse";

export type ResumeResponse = {
  id: number | null;
  accountId: number;
  resumeName: string;
  aiScore: number | null;
  aiScoreStaleness: string;
  personName: string;
  profession: string;
  email: string;
  phoneNumber: string;
  location: string;
  personalLinks: PersonalLinkResponse[];
  skills: SkillResponse[];
  education: ResumeEducationResponse[];
  experiences: ResumeExperienceResponse[];
  appearance: ResumeAppearanceResponse;
};
