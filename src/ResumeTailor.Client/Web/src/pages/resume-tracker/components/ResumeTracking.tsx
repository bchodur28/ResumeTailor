import { useState } from "react";
import type { ResumeSummaryQuery } from "../../../api/contracts/resumes/ResumeSummaryQuery";
import { ResumeSummaryDateFilter } from "../../../models/resumes/ResumeSummaryDateFilter";
import { ResumeSummarySortBy } from "../../../models/resumes/ResumeSummarySortBy";
import { useResumeSummaries } from "../../../hooks/useResumeSummeries";
import ResumeGrid from "./ResumeGrid";
import ResumeCard from "./ResumeCard";
import EmptyState from "../../../components/ui/feedback/EmptyState";
import { useNavigate } from "react-router-dom";
import ResumeTable from "./ResumeTable";

import TrackingToolBar from "./TrackingToolBar";

export type ResumeTrackingProps = {
  accountId: number;
};

const ResumeTracking = ({ accountId }: ResumeTrackingProps) => {
  const navigate = useNavigate();

  const [useTable, setUseTable] = useState(false);

  const [query, setQuery] = useState<Omit<ResumeSummaryQuery, "accountId">>({
    statuses: [],
    dateFilter: ResumeSummaryDateFilter.All,
    sortBy: ResumeSummarySortBy.AppliedDate,
    descending: true,
    page: 1,
    pageSize: 12,
  });

  const { data, isLoading, error } = useResumeSummaries({
    accountId: accountId,
    ...query,
  });

  const totalPages = data
    ? Math.ceil((data.totalCount ?? 0) / (query.pageSize ?? 12))
    : 0;

  const totalUnfilteredCount = data?.totalUnfilteredCount ?? 0;

  const totalCount = data?.totalCount ?? 0;

  const interestedCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "interested",
    ).length ?? 0;

  const interviewingCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "interviewing",
    ).length ?? 0;

  const offerReceivedCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "offerReceived",
    ).length ?? 0;

  const offerAcceptedCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "offerAccepted",
    ).length ?? 0;

  const appliedCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "applied",
    ).length ?? 0;

  const rejectedCount =
    data?.items?.filter(
      (resume) => resume.applicationTracking?.status === "rejected",
    ).length ?? 0;

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-semibold self-center">Resumes</h1>

      <div className="flex justify-between self-center sticky top-0  z-20 pt-4">
        <TrackingToolBar
          query={query}
          setQuery={setQuery}
          totalPages={totalPages}
          totalCount={totalCount}
          totalUnfilteredCount={totalUnfilteredCount}
          useTable={useTable}
          setUseTable={setUseTable}
          interestedCount={interestedCount}
          rejectedCount={rejectedCount}
          appliedCount={appliedCount}
          interviewingCount={interviewingCount}
          offerReceivedCount={offerReceivedCount}
          offerAcceptedCount={offerAcceptedCount}
        />
      </div>

      {isLoading ? (
        <div>Loading resume list items...</div>
      ) : error ? (
        <div>Error loading resume list items.</div>
      ) : totalUnfilteredCount === 0 ? (
        <EmptyState
          main="No resumes have been generated."
          secondary="Click below to generate a new resume."
        >
          <button onClick={() => navigate("/generate")} className="btn">
            Generate resume
          </button>
        </EmptyState>
      ) : data?.items.length === 0 ? (
        <EmptyState
          main="No resumes were found."
          secondary="Try adjusting your filters."
        />
      ) : data ? (
        <>
          {/* Table: only visible at 640px+ when selected */}
          {useTable && (
            <div className="hidden min-[640px]:block">
              <ResumeTable resumeSummaries={data.items} />
            </div>
          )}

          {/* Cards: visible on mobile or when table isn't selected */}
          <ResumeGrid className={useTable ? "min-[640px]:hidden" : ""}>
            {data.items.map((resume) => (
              <ResumeCard key={resume.id} resume={resume} />
            ))}
          </ResumeGrid>
        </>
      ) : null}
    </div>
  );
};

export default ResumeTracking;
