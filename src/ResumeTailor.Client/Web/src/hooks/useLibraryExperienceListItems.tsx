import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getLibraryExperienceListItems } from "../api/experienceApi";

export const useLibraryExperienceListItems = (archived: boolean = false) => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-experiences-list-items", archived],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getLibraryExperienceListItems(token, archived);
    },
  });
};
