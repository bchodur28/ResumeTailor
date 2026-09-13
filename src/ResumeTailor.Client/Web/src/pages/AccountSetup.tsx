import { useAccount } from "../contexts/AccountContext";
import { Navigate, useNavigate } from "react-router-dom";
import Card from "../components/ui/Card";
import Input from "../components/forms/Input";
import SearchableSelect from "../components/forms/SearchableSelect";

const AccountSetup = () => {
  const { account, isLoadingAccount, refreshAccount } = useAccount();
  const navigate = useNavigate();

  if (isLoadingAccount) {
    return <div>Loading...</div>;
  }

  if (account) {
    return <Navigate to="/" replace />;
  }

  const handleSubmit = async () => {
    await refreshAccount();
    navigate("/");
  };

  return (
    <div className="min-h-screen flex items-center justify-center">
      <Card className="w-1/2">
        <form className="flex flex-col gap-4 w-full">
          <Input id="email" label="Email*" required={true} />
          <Input id="displayName" label="Display Name*" required={true} />
          <Input id="country" label="Country*" required={true} />
          <SearchableSelect
            id="state"
            label="State*"
            options={[
              { id: 1, label: "California" },
              { id: 2, label: "New York" },
              { id: 3, label: "Texas" },
            ]} // Replace with actual options
            onChange={(option) => console.log(option)}
            isRequired={true}
          />
          <Input id="city" label="City*" required={true} />
          <button type="button" className="btn" onClick={handleSubmit}>
            Create Account
          </button>
        </form>
      </Card>
    </div>
  );
};

export default AccountSetup;
