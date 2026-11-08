import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getLibraryExperience } from "../api/experienceApi";

export const useLibraryExperience = (id: number) => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-experience", id],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getLibraryExperience(id, token);
    },
  });
};
