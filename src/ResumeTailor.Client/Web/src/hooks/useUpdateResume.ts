import { useMutation } from "@tanstack/react-query";
import { updateResume } from "../api/resumeApi";
import type { UpdateResumeRequest } from "../api/contracts/resumes/UpdateResumeRequest";

export const useUpdateResume = (resumeId: number) => {
  return useMutation({
    mutationFn: (request: UpdateResumeRequest) =>
      updateResume(resumeId, request),
  });
};
