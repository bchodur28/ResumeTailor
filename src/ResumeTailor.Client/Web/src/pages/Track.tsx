import { useResumeListItems } from "../hooks/useResumeListItems";
import { useAccount } from "../contexts/AccountContext";
import ListItem from "../components/ui/ListItem";
import { HouseDash } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";
import EmptyState from "../components/ui/EmptyState";
import type { ResumeListItemResponse } from "../api/contracts/resumes/ResumeListItemResponse";

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

  const createContentList = (
    resumeListItem: ResumeListItemResponse,
  ): string[] => {
    console.log(resumeListItem);
    const contentList = [resumeListItem.name];
    if (resumeListItem.jobPosting) {
      if (resumeListItem.jobPosting.jobTitle) {
        contentList.push(resumeListItem.jobPosting.jobTitle);
      }
      if (resumeListItem.jobPosting.location) {
        contentList.push(resumeListItem.jobPosting.location);
      }
      if (resumeListItem.jobPosting.workStyle) {
        contentList.push(resumeListItem.jobPosting.workStyle);
      }
      if (
        resumeListItem.jobPosting.salaryMin &&
        resumeListItem.jobPosting.salaryMax
      ) {
        contentList.push(
          `$${resumeListItem.jobPosting.salaryMin} - $${resumeListItem.jobPosting.salaryMax}`,
        );
      } else if (resumeListItem.jobPosting.salary) {
        contentList.push(`$${resumeListItem.jobPosting.salary}`);
      }
      if (resumeListItem.jobPosting.salaryPeriod) {
        contentList.push(`per ${resumeListItem.jobPosting.salaryPeriod}`);
      }
    }
    return contentList;
  };

  if (isLoading) {
    return <div>Loading resume list items...</div>;
  }
  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-semibold self-center">Resumes</h1>
      {(!data || data.length === 0) && (
        <EmptyState
          main="No resumes have been generated."
          secondary="Click below to generate a new resume."
        >
          <button onClick={() => navigate("/generate")} className="btn">
            Generate resume
          </button>
        </EmptyState>
      )}
      <ul className="flex flex-col gap-2">
        {data &&
          data.length > 0 &&
          data.map((resume) => (
            <ListItem
              item={{
                content: createContentList(resume),
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
