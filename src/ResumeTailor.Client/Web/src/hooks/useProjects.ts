import { useAuth0 } from "@auth0/auth0-react";
import { useQuery } from "@tanstack/react-query";
import { getProjects } from "../api/experience";

export const useProjects = () => {
  const { getAccessTokenSilently } = useAuth0();

  return useQuery({
    queryKey: ["projects"],
    queryFn: async () => {
      const token = await getAccessTokenSilently();
      return getProjects(token);
    },
  });
};
