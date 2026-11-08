import { Navigate, Outlet } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { useAccount } from "../../contexts/AccountContext";

const AccountRequiredRoute = () => {
  const { account, isLoadingAccount, accountError, refreshAccount } =
    useAccount();

  const { loginWithRedirect } = useAuth0();

  if (isLoadingAccount) {
    return <div>Loading...</div>;
  }

  if (accountError) {
    return (
      <div>
        <p>Failed to load account: {accountError}</p>

        <button type="button" onClick={() => void refreshAccount()}>
          Retry
        </button>

        <button type="button" onClick={() => void loginWithRedirect()}>
          Reconnect
        </button>
      </div>
    );
  }

  if (!account) {
    return <Navigate to="/account/setup" replace />;
  }

  return <Outlet />;
};

export default AccountRequiredRoute;
