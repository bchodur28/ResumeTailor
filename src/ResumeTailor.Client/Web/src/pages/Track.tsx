import { useAccount } from "../contexts/AccountContext";
import ResumeTracking from "../components/resume/ResumeTracking";

const Track = () => {
  const { account, isLoadingAccount } = useAccount();

  if (account == null) {
    return <div>No Account</div>;
  }

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  return <ResumeTracking accountId={account.id} />;
};

export default Track;
