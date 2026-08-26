export const jobFieldNames = [
  "jobTitle",
  "companyName",
  "location",
  "description",
  "salary",
  "employmentType",
  "postedDate",
  "applicationCount",
] as const;

export type JobFieldName = (typeof jobFieldNames)[number];
