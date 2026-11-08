import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getExperiences } from "../api/experienceApi";

export const useLibraryExperiences = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-experiences"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getExperiences(token);
    },
  });
};
