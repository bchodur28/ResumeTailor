import ResumeCard from "../components/resume/ResumeCard";
import { useAccount } from "../contexts/AccountContext";
import { useNavigate } from "react-router-dom";
import EmptyState from "../components/ui/EmptyState";
import { useResumeListItems } from "../hooks/useResumeListItems";
import ResumeGrid from "../components/resume/ResumeGrid";

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
      <ResumeGrid>
        {data &&
          data.length > 0 &&
          data.map((resume) => <ResumeCard key={resume.id} resume={resume} />)}
      </ResumeGrid>
    </div>
  );
};

export default Track;
