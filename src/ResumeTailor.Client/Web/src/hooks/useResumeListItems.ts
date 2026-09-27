import { useQuery } from "@tanstack/react-query";
import { getResumeListItems } from "../api/resumeApi";

export const useResumeListItems = (accountId: number) => {
  return useQuery({
    queryKey: ["resume-list-items", accountId],
    queryFn: () => getResumeListItems(accountId),
  });
};
