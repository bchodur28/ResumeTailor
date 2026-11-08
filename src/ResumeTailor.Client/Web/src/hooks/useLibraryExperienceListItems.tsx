import { useAuth0 } from "@auth0/auth0-react";
import { useQuery, type UseQueryResult } from "@tanstack/react-query";
import { getLibraryExperienceListItems } from "../api/experienceApi";

export const useLibraryExperienceListItems = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-experiences-list-items"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getLibraryExperienceListItems(token);
    },
  });
};
