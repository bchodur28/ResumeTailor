export const ExperienceType = {
  Employment: "employment",
  Internship: "internship",
  Freelance: "freelance",
  Contract: "contract",
  PersonalProject: "personalProject",
  AcademicProject: "academicProject",
  OpenSourceProject: "openSourceProject",
  Volunteer: "volunteer",
  Research: "research",
  Leadership: "leadership",
  Other: "other",
} as const;

export type ExperienceTypeValue =
  (typeof ExperienceType)[keyof typeof ExperienceType];

export const getExperienceTypeLabel = (value: ExperienceTypeValue): string => {
  return value
    .replace(/([A-Z])/g, " $1")
    .replace(/^./, (char) => char.toUpperCase());
};
