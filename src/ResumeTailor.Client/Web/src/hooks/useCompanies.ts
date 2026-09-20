import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getCompanies } from "../api/experience";

export const useCompanies = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["companies"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getCompanies(token);
    },
  });
};
