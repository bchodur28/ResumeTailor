import type { ResumeAppearanceRequest } from "./ResumeAppearanceRequest";
import type { ResumeCompanySelectionRequest } from "./ResumeCompanySelectionRequest";
import type { ResumeEducationSelectionRequest } from "./ResumeEducationSelectionRequest";
import type { ResumeProjectSelectionRequest } from "./ResumeProjectSelectionRequest";

export type UpdateResumeRequest = {
  name: string;
  companies: ResumeCompanySelectionRequest[];
  education: ResumeEducationSelectionRequest[];
  projects: ResumeProjectSelectionRequest[];
  appearance: ResumeAppearanceRequest;
};
