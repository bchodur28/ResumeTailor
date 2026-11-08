export const EducationType = {
  Degree: "degree",
  Certificate: "certificate",
  Certification: "certification",
  Other: "other",
} as const;

export type EducationTypeValue =
  (typeof EducationType)[keyof typeof EducationType];

export const getEducationTypeLabel = (value: EducationTypeValue): string => {
  return value
    .replace(/([A-Z])/g, " $1")
    .replace(/^./, (char) => char.toUpperCase());
};
