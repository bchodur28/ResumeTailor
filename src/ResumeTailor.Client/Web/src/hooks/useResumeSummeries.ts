import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getResumeSummaries } from "../api/resumeApi";
import type { ResumeSummaryQuery } from "../api/contracts/resumes/ResumeSummaryQuery";

export const useResumeSummaries = (request: ResumeSummaryQuery) => {
  return useQuery({
    queryKey: ["resume-summaries", request],
    queryFn: () => getResumeSummaries(request),
    placeholderData: keepPreviousData,
  });
};
