import Button from "../../../components/ui/buttons/Button";
import ButtonContainer from "../../../components/ui/buttons/ButtonContainer";
import DropdownButton from "../../../components/ui/buttons/DropdownButton";
import Radio from "../../../components/forms/Radio";
import Checkbox from "../../../components/forms/Checkbox";
import { ResumeSummaryDateFilter } from "../../../models/resumes/ResumeSummaryDateFilter";
import type { ResumeSummaryDateFilterValue } from "../../../models/resumes/ResumeSummaryDateFilter";
import {
  Files,
  Filter,
  Grid3x3Gap,
  Info,
  List,
  SortUp,
  Table,
} from "react-bootstrap-icons";
import type { ResumeSummarySortByValue } from "../../../models/resumes/ResumeSummarySortBy";
import { ResumeSummarySortBy } from "../../../models/resumes/ResumeSummarySortBy";
import type { ApplicationStatusValue } from "../../../models/resumes/ApplicationStatus";
import ActionList from "../../../components/ui/lists/ActionList";
import type { ResumeSummaryQuery } from "../../../api/contracts/resumes/ResumeSummaryQuery";
import type { Dispatch, SetStateAction } from "react";

export type TrackingQuery = Omit<ResumeSummaryQuery, "accountId">;

type TrackingToolBarProps = {
  query: TrackingQuery;
  setQuery: Dispatch<SetStateAction<TrackingQuery>>;
  totalPages: number;
  totalCount: number;
  totalUnfilteredCount: number;
  useTable: boolean;
  setUseTable: Dispatch<SetStateAction<boolean>>;
  interestedCount: number;
  rejectedCount: number;
  appliedCount: number;
  interviewingCount: number;
  offerReceivedCount: number;
  offerAcceptedCount: number;
};

const createStatItem = (label: string, count: number) => (
  <li className="flex gap-2 items-center">
    <p className="text-gray-700">{label}</p>
    <p>{count}</p>
  </li>
);

const TrackingToolBar = ({
  query,
  setQuery,
  totalPages,
  totalCount,
  totalUnfilteredCount,
  useTable,
  setUseTable,
  interestedCount,
  rejectedCount,
  appliedCount,
  interviewingCount,
  offerReceivedCount,
  offerAcceptedCount,
}: TrackingToolBarProps) => {
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

  return (
    <ButtonContainer hasStyle={true} align="space-between" isRelative={true}>
      <DropdownButton
        label="Filter"
        iconComponent={Filter}
        styleType="filter"
        containerAlignment="center"
        labelSlideOut={true}
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
                  onChange={(checked) => onStatusChange("interested", checked)}
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
        iconComponent={SortUp}
        styleType="filter"
        containerAlignment="center"
      >
        <div className="p-4">
          <div className="flex gap-2 w-64 justify-between">
            <div className="flex flex-col gap-2">
              <h2 className="text-md font-semibold text-gray-800">Sort By:</h2>
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
      {totalPages > 1 && (
        <DropdownButton
          label={`${query.page}/${totalPages}`}
          iconComponent={Files}
          hideLabel={false}
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
      )}
      <DropdownButton
        label={`${totalCount < query.pageSize ? totalCount : query.pageSize}`}
        styleType="filter"
        containerAlignment="center"
        iconComponent={List}
        hideLabel={false}
      >
        <ActionList
          actions={([12, 24, 48, "All"] as const).map((count) => ({
            actionName: `${count}`,
            actionFn: () =>
              onItemsPerPageChange(
                count === "All" ? (totalUnfilteredCount ?? 48) : count,
              ),
          }))}
        />
      </DropdownButton>
      <div className="hidden min-[640px]:block">
        <Button
          label={useTable ? "Switch to Grid" : "Switch to Table"}
          iconComponent={useTable ? Grid3x3Gap : Table}
          styleType="filter"
          labelSlideOut={true}
          onClick={() => {
            setQuery((prevQuery) => ({
              ...prevQuery,
              page: 1,
              pageSize: totalUnfilteredCount ?? 48,
            }));
            setUseTable(!useTable);
          }}
        />
      </div>

      <DropdownButton
        label="Stats (Per Page)"
        iconComponent={Info}
        styleType="filter"
        containerAlignment="center"
        labelSlideOut={true}
      >
        <ul className="flex flex-col justify-end gap-2 p-2 text-sm">
          {createStatItem("Total Resumes:", totalUnfilteredCount)}
          {createStatItem("Interested:", interestedCount)}
          {createStatItem("Applied:", appliedCount)}
          {createStatItem("Interviewing:", interviewingCount)}
          {createStatItem("Rejected:", rejectedCount)}
          {createStatItem("Offered:", offerReceivedCount)}
          {createStatItem("Accepted:", offerAcceptedCount)}
        </ul>
      </DropdownButton>
    </ButtonContainer>
  );
};

export default TrackingToolBar;
