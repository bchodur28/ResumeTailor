import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getCompanyBullets } from "../api/experienceApi";

export const useCompanyBullets = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["company-bullets"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getCompanyBullets(token);
    },
  });
};
