import { useQuery } from "@tanstack/react-query";
import { getResumeSummaries } from "../api/resumeApi";

export const useResumeSummaries = (accountId: number) => {
  return useQuery({
    queryKey: ["resume-summaries", accountId],
    queryFn: () => getResumeSummaries(accountId),
  });
};
