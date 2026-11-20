import { useQuery } from "@tanstack/react-query";
import { getResumeDetails } from "../api/resumeApi";
import { useAuth0 } from "@auth0/auth0-react";

export const useResumeDetails = (resumeId: number) => {
  const { getAccessTokenSilently } = useAuth0();
  return useQuery({
    queryKey: ["resume-details", resumeId],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getResumeDetails(resumeId, token);
    },
  });
};
