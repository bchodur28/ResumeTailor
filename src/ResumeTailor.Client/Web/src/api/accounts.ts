import type { AccountRequest } from "../models/api/AccountRequest";
import type { AccountResponse } from "../models/api/AccountResponse";

export const createAccount = async (request: AccountRequest, token: string) => {
  const response = await fetch("https://localhost:7139/api/accounts", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error("Failed to create account");
  }

  return response.json();
};

export const getAccount = async (
  token: string,
): Promise<AccountResponse | null> => {
  const response = await fetch("https://localhost:7139/api/accounts/me", {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (response.status === 404) {
    return null;
  }

  if (!response.ok) {
    throw new Error("Failed to load account.");
  }

  return await response.json();
};
