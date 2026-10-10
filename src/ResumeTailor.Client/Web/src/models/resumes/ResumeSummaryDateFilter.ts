export const ResumeSummaryDateFilter = {
  All: "All",
  Today: "Today",
  ThisWeek: "ThisWeek",
  ThisMonth: "ThisMonth",
} as const;

export type ResumeSummaryDateFilterValue =
  (typeof ResumeSummaryDateFilter)[keyof typeof ResumeSummaryDateFilter];
