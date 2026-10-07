import type { ResumeDetailsResponse } from "./contracts/resumes/ResumeDetailsResponse";
import type { ResumeSummaryResponse } from "./contracts/resumes/ResumeSummaryResponse";
import type { GenerateResumeRequest } from "./contracts/GenerateResumeRequest";
import type { UpdateResumeRequest } from "./contracts/resumes/UpdateResumeRequest";
import type { ApplicationStatusValue } from "../models/resumes/ApplicationStatus";

const resumeUrl = "https://localhost:7139/api/resumes/";

export const generateResume = async (
  accountId: number,
  request: GenerateResumeRequest,
): Promise<number> => {
  const response = await fetch(`${resumeUrl}${accountId}/generate`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume");
  }

  return await response.json();
};

export const getResumePdf = async (resumeId: number): Promise<Blob> => {
  const response = await fetch(`${resumeUrl}${resumeId}/pdf`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume PDF");
  }

  return await response.blob();
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
  const data = await response.json();
  console.log("Fetched resume details:", data);
  return data;
};

export const getResumeSummaries = async (
  accountId: number,
): Promise<ResumeSummaryResponse[]> => {
  const response = await fetch(`${resumeUrl}?accountId=${accountId}`, {
    method: "GET",
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resumes");
  }

  return await response.json();
};

export const updateResumeApplicationTracking = async (
  resumeId: number,
  status: ApplicationStatusValue,
): Promise<void> => {
  const response = await fetch(`${resumeUrl}${resumeId}/application-tracking`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(status),
  });

  if (!response.ok) {
    throw new Error("Failed to update resume application tracking status");
  }
};

export const updateResume = async (
  id: number,
  resume: UpdateResumeRequest,
): Promise<ResumeDetailsResponse> => {
  const response = await fetch(`${resumeUrl}${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(resume),
  });

  if (!response.ok) {
    throw new Error("Failed to update resume");
  }

  return response.json();
};
