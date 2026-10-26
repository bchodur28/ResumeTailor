import { useAccount } from "../../contexts/AccountContext";
import ResumeTracking from "./components/ResumeTracking";

const ResumeTrackingPage = () => {
  const { account, isLoadingAccount } = useAccount();

  if (account == null) {
    return <div>No Account</div>;
  }

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  return <ResumeTracking accountId={account.id} />;
};

export default ResumeTrackingPage;
