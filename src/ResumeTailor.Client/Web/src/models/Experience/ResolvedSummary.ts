export type ResolvedSummary = {
  resumeSummaryId: number | null;
  librarySummaryId: number;
  wasManuallyChanged: boolean;
  aiReasoning: string | null;
  sortOrder: number;
  value: string;
  useInResume: boolean;
};
