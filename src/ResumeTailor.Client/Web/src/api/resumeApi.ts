import type { ResumeDetailsResponse } from "./contracts/resumes/ResumeDetailsResponse";
import type { ResumeSummaryResponse } from "./contracts/resumes/ResumeSummaryResponse";
import type { ResumeSummaryQuery } from "./contracts/resumes/ResumeSummaryQuery";
import type { GenerateResumeRequest } from "./contracts/GenerateResumeRequest";
import type { UpdateResumeRequest } from "./contracts/resumes/UpdateResumeRequest";
import type { ApplicationStatusValue } from "../models/resumes/ApplicationStatus";
import type { PagedResult } from "../models/common/PagedResult";

const resumeUrl = "/api/resumes/";

export const generateResume = async (
  accountId: number,
  request: GenerateResumeRequest,
  token: string,
): Promise<number> => {
  const response = await fetch(`${resumeUrl}${accountId}/generate`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume");
  }

  return await response.json();
};

export const getResumePdf = async (
  resumeId: number,
  token: string,
): Promise<Blob> => {
  const response = await fetch(`${resumeUrl}${resumeId}/pdf`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume PDF");
  }

  return await response.blob();
};

export const getResumeDetails = async (
  resumeId: number,
  token: string,
): Promise<ResumeDetailsResponse> => {
  const response = await fetch(`${resumeUrl}${resumeId}`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resume details");
  }
  const data = await response.json();
  console.log("Fetched resume details:", data);
  return data;
};

export const getResumeSummaries = async (
  request: ResumeSummaryQuery,
  token: string,
): Promise<PagedResult<ResumeSummaryResponse>> => {
  const params = new URLSearchParams();

  params.append("accountId", request.accountId.toString());
  params.append("page", request.page.toString());
  params.append("pageSize", request.pageSize.toString());
  params.append("sortBy", request.sortBy);
  params.append("descending", request.descending.toString());
  params.append("dateFilter", request.dateFilter);

  request.statuses?.forEach((status) => {
    params.append("statuses", status);
  });

  const response = await fetch(`${resumeUrl}?${params.toString()}`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch resumes");
  }

  return await response.json();
};

export const updateResumeApplicationTracking = async (
  resumeId: number,
  status: ApplicationStatusValue,
  token: string,
): Promise<void> => {
  const response = await fetch(`${resumeUrl}${resumeId}/application-tracking`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
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
  token: string,
): Promise<ResumeDetailsResponse> => {
  const response = await fetch(`${resumeUrl}${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(resume),
  });

  if (!response.ok) {
    if (!response.ok) {
      const error = await response.text();
      console.error("Update resume failed:", response.status, error);
      throw new Error(`Failed to update resume: ${error}`);
    }
  }

  return response.json();
};
