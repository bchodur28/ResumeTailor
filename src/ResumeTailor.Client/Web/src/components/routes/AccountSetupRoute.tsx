import { Navigate, Outlet } from "react-router-dom";
import { useAccount } from "../../contexts/AccountContext";

const AccountSetupRoute = () => {
  const { account, isLoadingAccount, accountError, refreshAccount } =
    useAccount();

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
      </div>
    );
  }

  if (account) {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
};

export default AccountSetupRoute;
