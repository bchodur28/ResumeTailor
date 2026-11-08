import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getEducation } from "../api/educationApi";

export const useLibraryEducation = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["library-education"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getEducation(token);
    },
  });
};
