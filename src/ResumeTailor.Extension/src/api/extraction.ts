import type { FieldExtractionDefinition } from "../extraction/types/definitions/FieldExtractionDefinition";
import type { FieldExtractionDefinitionRequest } from "../extraction/types/requests/FieldExtractionDefinitionRequest";
import type { FieldSelector } from "../extraction/types/definitions/FieldSelector";
import type { FieldSelectorRequest } from "../extraction/types/requests/FieldSelectorRequest";
import type { SiteExtractionDefinition } from "../extraction/types/definitions/SiteExtractionDefinition";

const API_BASE_URL = "https://localhost:7139/api";

export const fetchSiteExtractionDefintion = async (pageUrl: URL) => {
  const query = new URLSearchParams({
    hostname: pageUrl.hostname,
    path: pageUrl.pathname,
  });

  const response = await fetch(
    `${API_BASE_URL}/SiteExtractionDefinition/match?${query}`,
  );

  if (!response.ok) {
    throw new Error(
      `Failed to fetch the site extraction definition: ${response.status}`,
    );
  }

  return await (response.json() as Promise<SiteExtractionDefinition>);
};

export const createFieldExtractionDefinition = async (
  field: FieldExtractionDefinitionRequest,
) => {
  const response = await fetch(`${API_BASE_URL}/FieldExtractionDefinition`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(field),
  });

  if (!response.ok) {
    throw new Error(
      `Failed to create field extraction definition. Status: ${response.status}`,
    );
  }

  return await (response.json() as Promise<SiteExtractionDefinition>);
};

export const createFieldSelector = async (request: FieldSelectorRequest) => {
  const response = await fetch(`${API_BASE_URL}/FieldSelector`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error(
      `Failed to create field selector. Status: ${response.status}`,
    );
  }

  return await (response.json() as Promise<FieldSelector>);
};

export const updateFieldExtractionDefinition = async (
  id: number,
  field: FieldExtractionDefinitionRequest,
): Promise<void> => {
  const response = await fetch(
    `${API_BASE_URL}/FieldExtractionDefinition/${id}`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(field),
    },
  );

  if (!response.ok) {
    throw new Error(
      `Failed to update field definition. Status: ${response.status}`,
    );
  }
};

export const updateFieldSelector = async (
  id: number,
  request: FieldSelectorRequest,
): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/FieldSelector/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error(
      `Failed to update field selector. Status: ${response.status}`,
    );
  }
};
