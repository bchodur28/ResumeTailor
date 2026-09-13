import { Navigate, Outlet } from "react-router-dom";
import { useAccount } from "../../contexts/AccountContext";

const AccountRequiredRoute = () => {
  const { account, isLoadingAccount } = useAccount();

  if (isLoadingAccount) {
    return <div>Loading...</div>;
  }

  if (!account) {
    return <Navigate to="/account/setup" />;
  }

  return <Outlet />;
};

export default AccountRequiredRoute;
