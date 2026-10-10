export const ResumeSummarySortBy = {
  AppliedDate: "AppliedDate",
  InterviewedDate: "InterviewedDate",
  Status: "Status",
  Salary: "Salary",
} as const;

export type ResumeSummarySortByValue =
  (typeof ResumeSummarySortBy)[keyof typeof ResumeSummarySortBy];
