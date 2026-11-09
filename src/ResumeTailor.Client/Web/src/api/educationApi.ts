import type { EducationRequest } from "./contracts/education/EducationRequest.ts";
import type { LibraryEducationResponse } from "./contracts/education/LibraryEducationResponse.ts";

const educationUrl = "/api/education";

export const getEducation = async (
  token: string,
  archived: boolean,
): Promise<LibraryEducationResponse[]> => {
  const response = await fetch(`${educationUrl}?archived=${archived}`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch education");
  }
  return await response.json();
};

export const createEducation = async (
  request: EducationRequest,
  token: string,
): Promise<void> => {
  const response = await fetch(educationUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create education");
  }
};

export const updateEducation = async (
  id: number,
  request: EducationRequest,
  token: string,
): Promise<void> => {
  const response = await fetch(`${educationUrl}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to update education");
  }
};

export const deleteEducation = async (
  id: number,
  token: string,
): Promise<void> => {
  const response = await fetch(`${educationUrl}/${id}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to delete education");
  }
};

export const archiveLibraryEducation = async (
  id: number,
  token: string,
): Promise<void> => {
  const response = await fetch(`${educationUrl}/${id}/archive`, {
    method: "PATCH",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    const problem = await response.json();
    throw new Error(problem.detail ?? "Failed to archive education");
  }
};

export const restoreLibraryEducation = async (
  id: number,
  token: string,
): Promise<void> => {
  const response = await fetch(`${educationUrl}/${id}/restore`, {
    method: "PATCH",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    const problem = await response.json();
    throw new Error(problem.detail ?? "Failed to restore education");
  }
};
