import { useResumeListItems } from "../hooks/useResumeListItems";
import { useAccount } from "../contexts/AccountContext";
import ListItem from "../components/ui/ListItem";
import { HouseDash } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";

const Track = () => {
  const { account, isLoadingAccount } = useAccount();
  const navigate = useNavigate();
  if (account == null) {
    return <div>No Account</div>;
  }

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  const { data, isLoading, error } = useResumeListItems(account.id);

  if (!data) {
    return <div>No resume list items found.</div>;
  }

  if (isLoading) {
    return <div>Loading resume list items...</div>;
  }
  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-semibold">Resumes</h1>
      <ul className="flex flex-col gap-2">
        {data.map((resume) => (
          <ListItem
            item={{
              content: [resume.name],
              icon: <HouseDash className="primary-color" />,
              actions: [
                {
                  actionName: "View",
                  actionFn: () => {
                    navigate(`/resumes/${resume.id}`);
                  },
                },
                {
                  actionName: "Rename",
                  actionFn: () => {
                    console.log(`Renaming ${resume.name}`);
                  },
                },
                {
                  actionName: "Delete",
                  actionFn: () => {
                    console.log(`Deleting ${resume.name}`);
                  },
                },
              ],
            }}
          >
            <ListItem.Icon />
            <ListItem.Content />
            <ListItem.Buttons />
          </ListItem>
        ))}
      </ul>
    </div>
  );
};

export default Track;
