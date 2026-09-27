import type { ResumeCompanyResult } from "../../../models/resumes/ResumeCompanyResult";
import type { PersonalLinkResponse } from "../accounts/PersonalLinkResponse";
import type { EducationResponse } from "../education/EducationResponse";
import type { ProjectResponse } from "../projects/ProjectResponse";

export type ResumeResponse = {
  id: number | null;
  accountId: number;
  personName: string;
  profession: string;
  email: string;
  phoneNumber: string;
  location: string;
  personalLinks: PersonalLinkResponse[];
  companies: ResumeCompanyResult[];
  education: EducationResponse[];
  projects: ProjectResponse[];
};
