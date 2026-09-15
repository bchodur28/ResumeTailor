import {
  createContext,
  useCallback,
  useContext,
  useState,
  useEffect,
  type PropsWithChildren,
} from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { getAccount } from "../api/accounts";
import type { AccountResponse } from "../models/api/AccountResponse";

type AccountContextType = {
  account: AccountResponse | null;
  isLoadingAccount: boolean;
  refreshAccount: () => Promise<void>;
};

const AccountContext = createContext<AccountContextType | undefined>(undefined);

export const AccountProvider = ({ children }: PropsWithChildren) => {
  const { isAuthenticated, getAccessTokenSilently } = useAuth0();

  const [account, setAccount] = useState<AccountResponse | null>(null);
  const [isLoadingAccount, setIsLoadingAccount] = useState<boolean>(true);

  const loadAccount = useCallback(async () => {
    if (!isAuthenticated) {
      setAccount(null);
      setIsLoadingAccount(false);
      return;
    }

    setIsLoadingAccount(true);

    try {
      const token = await getAccessTokenSilently({
        authorizationParams: { audience: import.meta.env.VITE_AUTH0_AUDIENCE },
      });

      const account = await getAccount(token);
      console.log("Loaded account:", account);
      setAccount(account);
    } finally {
      setIsLoadingAccount(false);
    }
  }, [isAuthenticated, getAccessTokenSilently]);

  useEffect(() => {
    loadAccount();
  }, [loadAccount]);

  return (
    <AccountContext.Provider
      value={{ account, isLoadingAccount, refreshAccount: loadAccount }}
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
