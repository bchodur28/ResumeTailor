import { useQuery } from "@tanstack/react-query";
import { getResumeDetails } from "../api/resumeApi";

export const useResumeDetails = (resumeId: number) => {
  return useQuery({
    queryKey: ["resume-details", resumeId],
    queryFn: () => getResumeDetails(resumeId),
  });
};
