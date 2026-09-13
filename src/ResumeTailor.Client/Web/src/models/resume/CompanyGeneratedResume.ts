import type { GeneratedCompany } from "./GeneratedCompany";
import type { Project } from "./Project";

export type CompanyGeneratedResume = {
  personName: string;
  profession: string;
  location: string;
  phoneNumber: string;
  college?: string;
  degree?: string;
  major?: string;
  collegeStatus?: string;
  personalSite1?: string;
  personalSite2?: string;
  personalSite3?: string;
  skills: string[];
  experience?: GeneratedCompany[];
  projects?: Project[];
};
