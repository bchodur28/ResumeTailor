import type { ExperienceTypeValue } from "../resumes/ExperienceType";

export type Experience = {
  type: ExperienceTypeValue;
  title: string;
  organization: string | null;
  location: string | null;
  started: string;
  ended: string | null;
  link: string | null;
  techStack: string | null;
};
