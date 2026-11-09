import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getEducation } from "../api/educationApi";

export const useLibraryEducation = (archived: boolean = false) => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-education", archived],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getEducation(token, archived);
    },
  });
};
