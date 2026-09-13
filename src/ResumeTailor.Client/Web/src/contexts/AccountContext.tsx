import {
  createContext,
  useContext,
  useState,
  useEffect,
  type PropsWithChildren,
} from "react";
import { useAuth0 } from "@auth0/auth0-react";

type Account = {
  id: number;
  email: string;
  displayName: string;
};

type AccountContextType = {
  account: Account | null;
  isLoadingAccount: boolean;
  refreshAccount: () => Promise<void>;
};

const AccountContext = createContext<AccountContextType | undefined>(undefined);

export const AccountProvider = ({ children }: PropsWithChildren) => {
  const { isAuthenticated, getAccessTokenSilently } = useAuth0();

  const [account, setAccount] = useState<Account | null>(null);
  const [isLoadingAccount, setIsLoadingAccount] = useState<boolean>(true);

  const loadAccount = async () => {
    if (!isAuthenticated) {
      setAccount(null);
      setIsLoadingAccount(false);
      return;
    }

    setIsLoadingAccount(true);

    try {
      const token = await getAccessTokenSilently();

      const response = await fetch("/api/accounts/me", {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status == 404) {
        setAccount(null);
        return;
      }

      if (!response.ok) {
        throw new Error("Failed to load account.");
      }

      const account = await response.json();
      setAccount(account);
    } finally {
      setIsLoadingAccount(false);
    }
  };

  useEffect(() => {
    loadAccount();
  }, [isAuthenticated]);

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
