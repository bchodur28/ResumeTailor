import type { ResumeDetailsResponse } from "./contracts/resumes/ResumeDetailsResponse";
import type { ResumeListItemResponse } from "./contracts/resumes/ResumeListItemResponse";

const resumeUrl = "https://localhost:7139/api/resumes/";

export const generateResume = async (accountId: number): Promise<number> => {
  const response = await fetch(`${resumeUrl}${accountId}/generate`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume");
  }

  return await response.json();
};

export const getResumeDetails = async (
  resumeId: number,
): Promise<ResumeDetailsResponse> => {
  const response = await fetch(`${resumeUrl}${resumeId}`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume details");
  }

  return await response.json();
};

export const getResumeListItems = async (
  accountId: number,
): Promise<ResumeListItemResponse[]> => {
  const response = await fetch(`${resumeUrl}account/${accountId}`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resumes");
  }

  return await response.json();
};
