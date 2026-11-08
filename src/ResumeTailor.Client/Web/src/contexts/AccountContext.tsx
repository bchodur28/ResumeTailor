import {
  createContext,
  useCallback,
  useContext,
  useState,
  useEffect,
  type PropsWithChildren,
} from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { getAccount } from "../api/accountsApi";
import type { AccountResponse } from "../api/contracts/accounts/AccountResponse";

type AccountContextType = {
  account: AccountResponse | null;
  isLoadingAccount: boolean;
  accountError: string | null;
  refreshAccount: () => Promise<void>;
};

const AccountContext = createContext<AccountContextType | undefined>(undefined);

export const AccountProvider = ({ children }: PropsWithChildren) => {
  const {
    isAuthenticated,
    isLoading: isLoadingAuth0,
    getAccessTokenSilently,
  } = useAuth0();

  const [account, setAccount] = useState<AccountResponse | null>(null);
  const [isLoadingAccount, setIsLoadingAccount] = useState<boolean>(true);
  const [accountError, setAccountError] = useState<string | null>(null);

  const loadAccount = useCallback(async () => {
    console.log("loadAccount called:", {
      isAuthenticated,
      isLoadingAuth0,
    });

    if (isLoadingAuth0) {
      setIsLoadingAccount(true);
      setAccountError(null);
      return;
    }

    if (!isAuthenticated) {
      console.log("Not authenticated, clearing account");
      setAccount(null);
      setAccountError(null);
      setIsLoadingAccount(false);
      return;
    }

    setIsLoadingAccount(true);
    setAccountError(null);

    try {
      console.log("1. Getting Auth0 token");

      const token = await getAccessTokenSilently();

      console.log("2. Auth0 token acquired");

      const loadedAccount = await getAccount(token);

      console.log("3. Account API response:", loadedAccount);

      setAccount(loadedAccount);

      console.log("4. Account state update requested");
    } catch (error) {
      console.error("Account loading failed:", error);
      setAccountError(
        error instanceof Error ? error.message : "Unable to load account",
      );
    } finally {
      console.log("5. Account loading finished");
      setIsLoadingAccount(false);
    }
  }, [isAuthenticated, isLoadingAuth0, getAccessTokenSilently]);

  useEffect(() => {
    loadAccount();
  }, [loadAccount]);

  return (
    <AccountContext.Provider
      value={{
        account,
        isLoadingAccount,
        accountError,
        refreshAccount: loadAccount,
      }}
    >
      {children}
    </AccountContext.Provider>
  );
};

export const useAccount = () => {
  const context = useContext(AccountContext);
  if (!context) {
    throw new Error("useAccount must be used within an AccountProvider");
  }
  return context;
};
