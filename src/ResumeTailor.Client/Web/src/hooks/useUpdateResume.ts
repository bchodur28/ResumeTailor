import { useMutation } from "@tanstack/react-query";
import { updateResume } from "../api/resumeApi";
import type { UpdateResumeRequest } from "../api/contracts/resumes/UpdateResumeRequest";
import { useAuth0 } from "@auth0/auth0-react";

export const useUpdateResume = (resumeId: number) => {
  const { getAccessTokenSilently } = useAuth0();
  return useMutation({
    mutationFn: async (request: UpdateResumeRequest) => {
      const token = await getAccessTokenSilently();
      return updateResume(resumeId, request, token);
    },
  });
};
