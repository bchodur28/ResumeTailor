import { useResumeListItems } from "../hooks/useResumeListItems";
import { useAccount } from "../contexts/AccountContext";
import ListItem from "../components/ui/ListItem";
import { FileEarmarkPerson, HouseDash } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";
import EmptyState from "../components/ui/EmptyState";
import type { ResumeListItemResponse } from "../api/contracts/resumes/ResumeListItemResponse";
import ActionDropdown from "../components/ui/ActionDropdown";

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
    const contentList = [];
    if (resumeListItem.jobPosting) {
      if (resumeListItem.jobPosting.companyName) {
        contentList.push(resumeListItem.jobPosting.companyName);
      }
      if (resumeListItem.jobPosting.jobTitle) {
        contentList.push(resumeListItem.jobPosting.jobTitle);
      }
      if (resumeListItem.jobPosting.location) {
        contentList.push(resumeListItem.jobPosting.location);
      }
      if (resumeListItem.jobPosting.workStyle) {
        const workStyle = resumeListItem.jobPosting.workStyle;
        const workStyleMod =
          workStyle.charAt(0).toUpperCase() + workStyle.slice(1);
        contentList.push(workStyleMod);
      }
      if (
        resumeListItem.jobPosting.salaryMin &&
        resumeListItem.jobPosting.salaryMax
      ) {
        const salaryPeriod = resumeListItem.jobPosting.salaryPeriod;
        const currencyType = resumeListItem.jobPosting.salaryCurrency;
        contentList.push(
          `$${resumeListItem.jobPosting.salaryMin.toLocaleString()} - $${resumeListItem.jobPosting.salaryMax.toLocaleString()} ${salaryPeriod ?? ""} ${currencyType ? `(${currencyType})` : ""}`,
        );
      } else if (resumeListItem.jobPosting.salary) {
        const salaryPeriod = resumeListItem.jobPosting.salaryPeriod;
        contentList.push(
          `$${resumeListItem.jobPosting.salary.toLocaleString()} ${salaryPeriod ?? ""}`,
        );
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
              content={resume.name}
              tags={createContentList(resume)}
              icon={<FileEarmarkPerson className="primary-color" />}
              actions={[
                {
                  actionName: "View",
                  actionFn: () => {
                    navigate(`/resumes/${resume.id}`);
                  },
                },
                {
                  actionName: "Edit Information",
                  actionFn: () => {
                    console.log(`Editing ${resume.name}`);
                  },
                  dropDownAction: true,
                },
                {
                  actionName: "Update Status",
                  actionFn: () => {
                    console.log(`Editing ${resume.name}`);
                  },
                  dropDownAction: true,
                },
                {
                  actionName: "Delete",
                  actionFn: () => {
                    console.log(`Deleting ${resume.name}`);
                  },
                  dropDownAction: true,
                },
              ]}
            />
          ))}
      </ul>
    </div>
  );
};

export default Track;
