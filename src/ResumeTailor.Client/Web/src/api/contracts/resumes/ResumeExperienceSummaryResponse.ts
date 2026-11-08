export type ResumeExperienceSummaryResponse = {
  id: number | null;
  librarySummaryId: number;
  wasManuallyChanged: boolean;
  aiReasoning: string | null;
  sortOrder: number;
  value: string;
};
