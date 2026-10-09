import ResumeCard from "../components/resume/ResumeCard";
import { useAccount } from "../contexts/AccountContext";
import { useNavigate } from "react-router-dom";
import EmptyState from "../components/ui/EmptyState";
import { useResumeSummaries } from "../hooks/useResumeSummeries";
import ResumeGrid from "../components/resume/ResumeGrid";
import FilterButton from "../components/ui/FilterButton";
import Button from "../components/ui/Button";
import { Check, Headphones } from "react-bootstrap-icons";
import DropdownButton from "../components/ui/DropdownButton";
import Checkbox from "../components/forms/Checkbox";
import Radio from "../components/forms/Radio";

const Track = () => {
  const { account, isLoadingAccount } = useAccount();
  const navigate = useNavigate();
  if (account == null) {
    return <div>No Account</div>;
  }

  if (isLoadingAccount) {
    return <div>Loading account...</div>;
  }

  const { data, isLoading, error } = useResumeSummaries(account.id);

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
      <div className="flex justify-between self-center gap-4">
        <DropdownButton
          label="Filter"
          hideLabel={false}
          styleType="filter"
          containerAlignment="center"
        >
          <div className="p-4">
            <div className="flex gap-2 w-64 justify-between">
              <div className="flex flex-col gap-2">
                <h2 className="text-md font-semibold text-gray-800">
                  Filter by status
                </h2>
                <div className="flex flex-col gap-2 pl-4">
                  <Checkbox id="interested" label="Interested" />
                  <Checkbox id="applied" label="Applied" />
                  <Checkbox id="rejected" label="Rejected" />
                  <Checkbox id="offered" label="Offered" />
                  <Checkbox id="accepted" label="Accepted" />
                </div>
              </div>
              <div className="flex flex-col gap-2">
                <h2 className="text-md font-semibold text-gray-800">
                  Filter by date
                </h2>
                <div className="flex flex-col gap-2 pl-4">
                  <Radio id="Today" label="Today" />
                  <Radio id="ThisWeek" label="This Week" />
                  <Radio id="ThisMonth" label="This Month" />
                  <Radio id="all" label="All" />
                </div>
              </div>
            </div>
          </div>
        </DropdownButton>
        <DropdownButton
          label="Sort"
          hideLabel={false}
          styleType="filter"
          containerAlignment="center"
        >
          <div className="p-4">
            <div className="flex gap-2 w-64 justify-between">
              <div className="flex flex-col gap-2">
                <h2 className="text-md font-semibold text-gray-800">
                  Sort By:
                </h2>
                <div className="flex flex-col gap-2 pl-4">
                  <Radio id="applied-newest" label="Applied On (Newest)" />
                  <Radio id="applied-oldest" label="Applied On (Oldest)" />
                  <Radio
                    id="interviewed-newest"
                    label="Interviewed On (Newest)"
                  />
                  <Radio
                    id="interviewed-oldest"
                    label="Interviewed On (Oldest)"
                  />
                  <Radio
                    id="status-order-rejected"
                    label="Status (Rejected -> Accepted)"
                  />
                  <Radio
                    id="status-order-accepted"
                    label="Status (Accepted -> Rejected)"
                  />
                  <Radio id="salary" label="Salary" />
                </div>
              </div>
            </div>
          </div>
        </DropdownButton>
      </div>

      <ResumeGrid>
        {data &&
          data.length > 0 &&
          data.map((resume) => <ResumeCard key={resume.id} resume={resume} />)}
      </ResumeGrid>
    </div>
  );
};

export default Track;
