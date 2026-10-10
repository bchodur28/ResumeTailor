import { useState } from "react";
import type { ResumeSummaryQuery } from "../../api/contracts/resumes/ResumeSummaryQuery";
import {
  ResumeSummaryDateFilter,
  type ResumeSummaryDateFilterValue,
} from "../../models/resumes/ResumeSummaryDateFilter";
import {
  ResumeSummarySortBy,
  type ResumeSummarySortByValue,
} from "../../models/resumes/ResumeSummarySortBy";
import type { ApplicationStatusValue } from "../../models/resumes/ApplicationStatus";
import { useResumeSummaries } from "../../hooks/useResumeSummeries";
import ResumeGrid from "./ResumeGrid";
import ResumeCard from "./ResumeCard";
import DropdownButton from "../ui/DropdownButton";
import Radio from "../forms/Radio";
import Checkbox from "../forms/Checkbox";
import EmptyState from "../ui/EmptyState";
import { useNavigate } from "react-router-dom";

export type ResumeTrackingProps = {
  accountId: number;
};

const ResumeTracking = ({ accountId }: ResumeTrackingProps) => {
  const navigate = useNavigate();

  const [query, setQuery] = useState<Omit<ResumeSummaryQuery, "accountId">>({
    statuses: [],
    dateFilter: ResumeSummaryDateFilter.All,
    sortBy: ResumeSummarySortBy.AppliedDate,
    descending: true,
    page: 1,
    pageSize: 12,
  });

  const onPageChange = (page: number) => {
    setQuery((prev) => ({
      ...prev,
      page,
    }));
  };

  const onDateFilterChange = (value: ResumeSummaryDateFilterValue) => {
    setQuery((prev) => ({
      ...prev,
      dateFilter: value,
      page: 1,
    }));
  };

  const onSortChange = (
    sortBy: ResumeSummarySortByValue,
    descending: boolean,
  ) => {
    setQuery((prev) => ({
      ...prev,
      sortBy,
      descending,
      page: 1,
    }));
  };

  const onStatusChange = (status: ApplicationStatusValue, checked: boolean) => {
    setQuery((prev) => ({
      ...prev,
      statuses: checked
        ? [...(prev.statuses ?? []), status]
        : (prev.statuses ?? []).filter((s) => s !== status),
      page: 1,
    }));
  };

  const onItemsPerPageChange = (pageSize: number) => {
    setQuery((prev) => ({
      ...prev,
      pageSize,
      page: 1,
    }));
  };

  const getSortLabel = () => {
    switch (query.sortBy) {
      case ResumeSummarySortBy.AppliedDate:
        return `Sort: Applied On ${query.descending ? "(Newest)" : "(Oldest)"}`;
      case ResumeSummarySortBy.InterviewedDate:
        return `Sort: Interviewed On ${query.descending ? "Newest" : "Oldest"}`;
      default:
        return "Sort";
    }
  };

  const { data, isLoading, error } = useResumeSummaries({
    accountId: accountId,
    ...query,
  });

  const totalPages = data
    ? Math.ceil((data.totalCount ?? 0) / (query.pageSize ?? 12))
    : 0;

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-semibold self-center">Resumes</h1>

      <div className="flex justify-between self-center gap-4">
        <DropdownButton
          label="Filter"
          hideLabel={false}
          styleType="filter"
          containerAlignment="center"
        >
          <div className="p-4 flex flex-col gap-4">
            <div className="flex gap-2 w-64 justify-between">
              <div className="flex flex-col gap-2">
                <h2 className="text-md font-semibold text-gray-800">
                  Filter by status
                </h2>
                <div className="flex flex-col gap-2 pl-4">
                  <Checkbox
                    id="interested"
                    label="Interested"
                    checked={query.statuses?.includes("interested") ?? false}
                    onChange={(checked) =>
                      onStatusChange("interested", checked)
                    }
                  />
                  <Checkbox
                    id="applied"
                    label="Applied"
                    checked={query.statuses?.includes("applied") ?? false}
                    onChange={(checked) => onStatusChange("applied", checked)}
                  />
                  <Checkbox
                    id="rejected"
                    label="Rejected"
                    checked={query.statuses?.includes("rejected") ?? false}
                    onChange={(checked) => onStatusChange("rejected", checked)}
                  />
                  <Checkbox
                    id="offerReceived"
                    label="Offered"
                    checked={query.statuses?.includes("offerReceived") ?? false}
                    onChange={(checked) =>
                      onStatusChange("offerReceived", checked)
                    }
                  />
                  <Checkbox
                    id="offerAccepted"
                    label="Accepted"
                    checked={query.statuses?.includes("offerAccepted") ?? false}
                    onChange={(checked) =>
                      onStatusChange("offerAccepted", checked)
                    }
                  />
                </div>
              </div>
              <div className="flex flex-col gap-2">
                <h2 className="text-md font-semibold text-gray-800">
                  Filter by date
                </h2>
                <div className="flex flex-col gap-2 pl-4">
                  <Radio
                    id="Today"
                    label="Today"
                    name="dateFilter"
                    checked={query.dateFilter === "Today"}
                    onChange={() => {
                      onDateFilterChange(ResumeSummaryDateFilter.Today);
                    }}
                  />
                  <Radio
                    id="ThisWeek"
                    label="This Week"
                    name="dateFilter"
                    checked={query.dateFilter === "ThisWeek"}
                    onChange={() => {
                      onDateFilterChange(ResumeSummaryDateFilter.ThisWeek);
                    }}
                  />
                  <Radio
                    id="ThisMonth"
                    label="This Month"
                    name="dateFilter"
                    checked={query.dateFilter === "ThisMonth"}
                    onChange={() => {
                      onDateFilterChange(ResumeSummaryDateFilter.ThisMonth);
                    }}
                  />
                  <Radio
                    id="all"
                    label="All"
                    name="dateFilter"
                    checked={query.dateFilter === "All"}
                    onChange={() => {
                      onDateFilterChange(ResumeSummaryDateFilter.All);
                    }}
                  />
                </div>
              </div>
            </div>
          </div>
        </DropdownButton>
        <DropdownButton
          label={getSortLabel()}
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
                  <Radio
                    id="applied-newest"
                    label="Applied On (Newest)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.AppliedDate &&
                      query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.AppliedDate, true)
                    }
                  />
                  <Radio
                    id="applied-oldest"
                    label="Applied On (Oldest)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.AppliedDate &&
                      !query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.AppliedDate, false)
                    }
                  />
                  <Radio
                    id="interviewed-newest"
                    label="Interviewed On (Newest)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.InterviewedDate &&
                      query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.InterviewedDate, true)
                    }
                  />
                  <Radio
                    id="interviewed-oldest"
                    label="Interviewed On (Oldest)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.InterviewedDate &&
                      !query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.InterviewedDate, false)
                    }
                  />
                  <Radio
                    id="status-order-rejected"
                    label="Status (Rejected -> Accepted)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.Status &&
                      !query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.Status, false)
                    }
                  />
                  <Radio
                    id="status-order-accepted"
                    label="Status (Accepted -> Rejected)"
                    name="sortOption"
                    checked={
                      query.sortBy === ResumeSummarySortBy.Status &&
                      query.descending
                    }
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.Status, true)
                    }
                  />
                  <Radio
                    id="salary"
                    label="Salary"
                    name="sortOption"
                    checked={query.sortBy === ResumeSummarySortBy.Salary}
                    onChange={() =>
                      onSortChange(ResumeSummarySortBy.Salary, true)
                    }
                  />
                </div>
              </div>
            </div>
          </div>
        </DropdownButton>

        <DropdownButton
          label={`Page: ${query.page}`}
          styleType="filter"
          containerAlignment="center"
        >
          <ul className="flex flex-col justify-end">
            {Array.from({ length: totalPages }, (_, index) => (
              <button
                className="p-2 bg-white hover:bg-gray-100 text-gray-800 text-sm font-semibold"
                key={index}
                onClick={() => onPageChange(index + 1)}
              >
                {index + 1}
              </button>
            ))}
          </ul>
        </DropdownButton>
        <DropdownButton
          label={`Page Size: ${query.pageSize}`}
          styleType="filter"
          containerAlignment="center"
        >
          <ul className="flex flex-col justify-end">
            {[12, 24, 48].map((count) => (
              <button
                className="p-2 bg-white hover:bg-gray-100 text-gray-800 text-sm font-semibold"
                key={count}
                onClick={() => onItemsPerPageChange(count)}
              >
                {count}
              </button>
            ))}
          </ul>
        </DropdownButton>
      </div>
      {isLoading ? (
        <div>Loading resume list items...</div>
      ) : error ? (
        <div>Error loading resume list items.</div>
      ) : (
        <ResumeGrid>
          {data?.items.map((resume) => (
            <ResumeCard key={resume.id} resume={resume} />
          ))}
        </ResumeGrid>
      )}
      {(!data?.items || data.items.length === 0) && (
        <EmptyState
          main="No resumes have been generated."
          secondary="Click below to generate a new resume."
        >
          <button onClick={() => navigate("/generate")} className="btn">
            Generate resume
          </button>
        </EmptyState>
      )}
      <div className="flex justify-center mt-4">
        <DropdownButton
          label={`Page: ${query.page}`}
          styleType="filter"
          containerAlignment="center"
        >
          <ul className="flex flex-col justify-end">
            {Array.from({ length: totalPages }, (_, index) => (
              <button
                className="p-2 bg-white hover:bg-gray-100 text-gray-800 text-sm font-semibold"
                key={index}
                onClick={() => onPageChange(index + 1)}
              >
                {index + 1}
              </button>
            ))}
          </ul>
        </DropdownButton>
      </div>
    </div>
  );
};

export default ResumeTracking;
