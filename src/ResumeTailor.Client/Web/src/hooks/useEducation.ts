import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getEducation } from "../api/educationApi";

export const useEducation = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["education"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getEducation(token);
    },
  });
};
