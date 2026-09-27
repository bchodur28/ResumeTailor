import type { EducationRequest } from "./contracts/education/EducationRequest.ts";
import type { EducationResponse } from "./contracts/education/EducationResponse.ts";

const educationUrl = "https://localhost:7139/api/education/me";

export const getEducation = async (
  token: string,
): Promise<EducationResponse[]> => {
  const response = await fetch(educationUrl, {
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
  request: EducationRequest[],
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
  request: EducationRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(educationUrl, {
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
  ids: number[],
  token: string,
): Promise<void> => {
  const response = await fetch(educationUrl, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(ids),
  });

  if (!response.ok) {
    throw new Error("Failed to delete education");
  }
};
