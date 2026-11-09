import type { PersonalLinkResponse } from "../../api/contracts/accounts/PersonalLinkResponse";
import type { SkillResponse } from "../../api/contracts/accounts/SkillResponse";
import type { ResumeAppearanceResponse } from "../../api/contracts/resumes/ResumeAppearanceResponse";
import type { ResolvedEducation } from "../education/ResolvedEducation";
import type { ResolvedExperience } from "../Experience/ResolvedExperience";

export type ResolvedResume = {
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
  summary?: string;
  personalLinks: PersonalLinkResponse[];
  skills: SkillResponse[];
  education: ResolvedEducation[];
  experiences: ResolvedExperience[];
  appearance: ResumeAppearanceResponse;
};
