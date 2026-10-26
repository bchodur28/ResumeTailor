import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getExperienceSummaries } from "../api/experienceApi";

export const useExperienceSummaries = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["experience-summaries"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getExperienceSummaries(token);
    },
  });
};
