import EditCompanyBulletsForm from "../components/profile/EditCompanyBulletsForm";

const Manage = () => {
  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-semibold">Manage Bullets</h1>
      <EditCompanyBulletsForm />
    </div>
  );
};

export default Manage;
