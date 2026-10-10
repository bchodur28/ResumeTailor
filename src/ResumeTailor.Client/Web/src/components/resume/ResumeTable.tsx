import React from "react";
import type { ResumeSummaryResponse } from "../../api/contracts/resumes/ResumeSummaryResponse";

export type ResumeTableProps = {
  resumeSummaries: ResumeSummaryResponse[];
};

const uppercaseFirstLetter = (str: string) =>
  str.charAt(0).toUpperCase() + str.slice(1);

const formatDate = (date: string | Date) => {
  let value: Date;

  if (typeof date === "string") {
    const [year, month, day] = date.split("-").map(Number);
    value = new Date(year, month - 1, day);
  } else {
    value = date;
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "long",
    day: "numeric",
    year: "numeric",
  }).format(value);
};

const formatSalary = (
  salaryMin?: number | null,
  salaryMax?: number | null,
  salaryPeriod?: string | null,
  salaryCurrency?: string | null,
  salary?: number | null,
) => {
  if (salaryMin && salaryMax) {
    return `$${salaryMin.toLocaleString()} - $${salaryMax.toLocaleString()} ${uppercaseFirstLetter(salaryPeriod ?? "")} ${salaryCurrency ? `(${salaryCurrency})` : ""}`;
  }
  if (salary) {
    return `$${salary.toLocaleString()} ${uppercaseFirstLetter(salaryPeriod ?? "")}`;
  }
  return "Not specified";
};

const createTableCell = (content: React.ReactNode) => (
  <td className="py-2 px-4 pr-12">{content}</td>
);

const createTableHeaderCell = (content: React.ReactNode) => (
  <th className="p-4 pr-12 text-left">{content}</th>
);

const ResumeTable = ({ resumeSummaries }: ResumeTableProps) => {
  return (
    <div className="max-h-225 overflow-y-auto bg-gray-100 shadow-md rounded-2xl">
      <table className="w-full table-fixed">
        <thead className="sticky top-0 z-10 bg-gray-200">
          <tr>
            {createTableHeaderCell("Company")}
            {createTableHeaderCell("Job Title")}
            {createTableHeaderCell("Status")}
            {createTableHeaderCell("Applied Date")}
            {createTableHeaderCell("Salary")}
            {createTableHeaderCell("Location")}
            {createTableHeaderCell("Work Style")}
          </tr>
        </thead>
        <tbody>
          {resumeSummaries.map((resume) => (
            <tr
              key={resume.id}
              className="border-b border-gray-400 font-semibold text-gray-700 hover:bg-gray-200 hover:font-bold hover:text-gray-900"
            >
              {createTableCell(
                resume.jobPosting?.companyName ?? "Not specified",
              )}
              {createTableCell(resume.jobPosting?.jobTitle ?? "Not specified")}
              {createTableCell(resume.applicationTracking?.status)}
              {createTableCell(
                resume.applicationTracking?.applied
                  ? formatDate(resume.applicationTracking.applied)
                  : "Haven't Applied",
              )}
              {createTableCell(
                formatSalary(
                  resume.jobPosting?.salaryMin,
                  resume.jobPosting?.salaryMax,
                  resume.jobPosting?.salaryPeriod,
                  resume.jobPosting?.salaryCurrency,
                  resume.jobPosting?.salary,
                ),
              )}
              {createTableCell(resume.jobPosting?.location ?? "Not specified")}
              {createTableCell(
                uppercaseFirstLetter(
                  resume.jobPosting?.workStyle ?? "Not specified",
                ),
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ResumeTable;
