import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { getResumeSummaries } from "../api/resumeApi";
import { useAuth0 } from "@auth0/auth0-react";
import type { ResumeSummaryQuery } from "../api/contracts/resumes/ResumeSummaryQuery";

export const useResumeSummaries = (request: ResumeSummaryQuery) => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["resume-summaries", request],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getResumeSummaries(request, token);
    },
    placeholderData: keepPreviousData,
  });
};
