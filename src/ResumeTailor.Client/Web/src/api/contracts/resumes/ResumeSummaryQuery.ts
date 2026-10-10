import type { ResumeSummaryDateFilterValue } from "../../../models/resumes/ResumeSummaryDateFilter";
import type { ApplicationStatusValue } from "../../../models/resumes/ApplicationStatus";
import type { ResumeSummarySortByValue } from "../../../models/resumes/ResumeSummarySortBy";

export type ResumeSummaryQuery = {
  accountId: number;

  statuses: ApplicationStatusValue[] | null;
  dateFilter: ResumeSummaryDateFilterValue;

  sortBy: ResumeSummarySortByValue;

  descending: boolean;

  page: number;
  pageSize: number;
};
