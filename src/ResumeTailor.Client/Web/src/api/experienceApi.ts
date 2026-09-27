import type { CompanyRequest } from "./contracts/companies/CompanyRequest.ts";
import type { CompanyResponse } from "./contracts/companies/CompanyResponse.ts";
import type { CompanyBulletsResponse } from "./contracts/companies/CompanyBulletsResponse.ts";
import type { ProjectRequest } from "./contracts/projects/ProjectRequest.ts";
import type { ProjectResponse } from "./contracts/projects/ProjectResponse.ts";
import type { BulletRequest } from "./contracts/bullets/BulletRequest.ts";
import type { BulletDeleteRequest } from "./contracts/bullets/BulletDeleteRequest.ts";

const companyUrl = "https://localhost:7139/api/experience/me/companies";
const projectUrl = "https://localhost:7139/api/experience/me/projects";
const bulletUrl = "https://localhost:7139/api/experience/me/bullets";

// Companies API functions
export const getCompanies = async (
  token: string,
): Promise<CompanyResponse[]> => {
  const response = await fetch(companyUrl, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch companies");
  }
  return await response.json();
};

export const createCompanies = async (
  request: CompanyRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(companyUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create companies");
  }
};

export const updateCompanies = async (
  request: CompanyRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(companyUrl, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to update companies");
  }
};

export const deleteCompanies = async (
  ids: number[],
  token: string,
): Promise<void> => {
  const response = await fetch(companyUrl, {
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

// Projects API functions
export const getProjects = async (
  token: string,
): Promise<ProjectResponse[]> => {
  const response = await fetch(projectUrl, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch projects");
  }
  return await response.json();
};

export const createProjects = async (
  request: ProjectRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(projectUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create projects");
  }
};

export const updateProjects = async (
  request: ProjectRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(projectUrl, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to update projects");
  }
};

export const deleteProjects = async (
  ids: number[],
  token: string,
): Promise<void> => {
  const response = await fetch(projectUrl, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(ids),
  });

  if (!response.ok) {
    throw new Error("Failed to delete projects");
  }
};

// Bullets API functions
export const getCompanyBullets = async (
  token: string,
): Promise<CompanyBulletsResponse[]> => {
  const response = await fetch(bulletUrl, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to fetch companies with bullets");
  }
  return await response.json();
};

export const createBullets = async (
  request: BulletRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(bulletUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create bullets");
  }
};

export const updateBullets = async (
  request: BulletRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(bulletUrl, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to update bullets");
  }
};

export const deleteBullets = async (
  request: BulletDeleteRequest[],
  token: string,
): Promise<void> => {
  const response = await fetch(bulletUrl, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to delete bullets");
  }
};
