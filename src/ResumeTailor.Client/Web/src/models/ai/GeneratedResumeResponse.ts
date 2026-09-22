import type { PersonalLinkResponse } from "../profile/PersonalLinkResponse";

export type GeneratedResumeResponse = {
  id: string | null;
  accountId: number;
  personName: string;
  profession: string;
  email: string;
  phoneNumber: string;
  location: string;
  personalLinks: PersonalLinkResponse[];
  companies: ResumeCompanyResult;
};
