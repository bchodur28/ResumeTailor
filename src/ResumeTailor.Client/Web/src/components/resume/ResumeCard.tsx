import { Link } from "react-router-dom";
import type { ResumeSummaryResponse } from "../../api/contracts/resumes/ResumeSummaryResponse";
import {
  ApplicationStatus,
  type ApplicationStatusValue,
} from "../../models/resumes/ApplicationStatus";
import ActionDropdown from "../ui/ActionDropdown";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { updateResumeApplicationTracking } from "../../api/resumeApi";
import Message from "../ui/Message";
import applicationIcon from "../../assets/icons/application.png";
import interviewingIcon from "../../assets/icons/job-interview.png";
import handShakeIcon from "../../assets/icons/handshake.png";
import rejectedIcon from "../../assets/icons/reject.png";
import DropdownButton from "../ui/DropdownButton";
import ActionList from "../ui/ActionList";

type ResumeCardProps = {
  resume: ResumeSummaryResponse;
};

const uppercaseFirstLetter = (str: string) =>
  str.charAt(0).toUpperCase() + str.slice(1);

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

const getDisplayName = (name: string) => {
  return name.replace(/_\d{4}_\d{2}_\d{2}$/, "").replaceAll("_", " ");
};

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

const getStatusColor = (status: ApplicationStatusValue | undefined) => {
  console.log("Current status:", status);
  if (!status) {
    return ["var(--primary-color)", "white"];
  }
  switch (status) {
    case ApplicationStatus.Applied:
      return ["var(--applied-color)", "white"];
    case ApplicationStatus.Interviewed:
      return ["var(--interviewed-color)", "black"];
    case ApplicationStatus.OfferReceived:
      return ["var(--offer-received-color)", "black"];
    case ApplicationStatus.OfferAccepted:
      return ["var(--offer-accepted-color)", "white"];
    case ApplicationStatus.Rejected:
      return ["var(--rejected-color)", "white"];
    default:
      return ["var(--primary-color)", "white"];
  }
};

function renderInfo(title: string, value: string | null | undefined) {
  return (
    <div className="flex gap-2">
      <p className="font-semibold text-gray-600 text-sm">{title}:</p>
      <p className="text-gray-800 text-sm font-semibold">
        {uppercaseFirstLetter(value ?? "Not specified")}
      </p>
    </div>
  );
}

const getLocalDateString = () => {
  const date = new Date();

  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
};

const ResumeCard = ({ resume }: ResumeCardProps) => {
  const [statusUpdatedSuccessfully, setStatusUpdatedSuccessfully] = useState<
    boolean | null
  >(null);
  const [currentStatus, setCurrentStatus] = useState(
    resume.applicationTracking?.status,
  );
  const [appliedOn, setAppliedOn] = useState(
    resume.applicationTracking?.applied,
  );
  const [interviewedOn, setInterviewedOn] = useState(
    resume.applicationTracking?.interviewed,
  );
  const [offerReceivedOn, setOfferReceivedOn] = useState(
    resume.applicationTracking?.offerReceived,
  );
  const [offerAcceptedOn, setOfferAcceptedOn] = useState(
    resume.applicationTracking?.offerAccepted,
  );
  const [rejectedOn, setRejectedOn] = useState(
    resume.applicationTracking?.rejected,
  );

  const setDateForStatus = (status: ApplicationStatusValue, date: string) => {
    switch (status) {
      case ApplicationStatus.Applied:
        setAppliedOn(date);
        break;
      case ApplicationStatus.Interviewed:
        setInterviewedOn(date);
        break;
      case ApplicationStatus.OfferReceived:
        setOfferReceivedOn(date);
        break;
      case ApplicationStatus.OfferAccepted:
        setOfferAcceptedOn(date);
        break;
      case ApplicationStatus.Rejected:
        setRejectedOn(date);
        break;
    }
  };
  const saveStatus = useMutation({
    mutationFn: (status: ApplicationStatusValue) =>
      updateResumeApplicationTracking(resume.id, status),

    onSuccess: (_, status) => {
      setCurrentStatus(status);
      setDateForStatus(status, getLocalDateString());
      setStatusUpdatedSuccessfully(true);
    },
    onError: (error: string) => {
      console.error("Failed to update status:", error);
      setStatusUpdatedSuccessfully(false);
    },
  });

  const handleStatusChange = (status: ApplicationStatusValue) => {
    saveStatus.mutate(status);
  };

  return (
    <div className="resume-card">
      <Link className="flex flex-col gap-2 flex-1" to={`/resumes/${resume.id}`}>
        <div
          style={{
            backgroundColor: getStatusColor(currentStatus)[0],
            color: getStatusColor(currentStatus)[1],
          }}
          className="flex items-center p-3 rounded-t-lg shadow-md"
        >
          <h1 className="text-md font-semibold">
            {getDisplayName(resume.name)}
          </h1>
        </div>
        <div>
          {statusUpdatedSuccessfully === true && (
            <Message
              type="success"
              message="Status updated successfully"
              onClose={() => setStatusUpdatedSuccessfully(null)}
            />
          )}
          {statusUpdatedSuccessfully === false && (
            <Message
              type="error"
              message="Failed to update status"
              onClose={() => setStatusUpdatedSuccessfully(null)}
            />
          )}
        </div>

        <div className="grid grid-cols-3 px-4 justify-between gap-6">
          {appliedOn && (
            <img
              src={applicationIcon}
              alt="Applied"
              className="h-16 w-16 object-cover"
            />
          )}
          {interviewedOn && (
            <img
              src={interviewingIcon}
              alt="Interviewing"
              className="h-16 w-16 object-cover"
            />
          )}
          {offerReceivedOn && (
            <img
              src={handShakeIcon}
              alt="Offer Received"
              className="h-16 w-16 object-cover"
            />
          )}
          {rejectedOn && (
            <img
              src={rejectedIcon}
              alt="Offer Rejected"
              className="h-16 w-16 object-cover"
            />
          )}
        </div>
        <div className="flex-1 px-4">
          {renderInfo("Company", resume.jobPosting?.companyName)}
          {renderInfo("Job Title", resume.jobPosting?.jobTitle)}
          {renderInfo("Location", resume.jobPosting?.location)}
          {renderInfo("Work Style", resume.jobPosting?.workStyle)}
          {renderInfo(
            "Salary",
            formatSalary(
              resume.jobPosting?.salaryMin,
              resume.jobPosting?.salaryMax,
              resume.jobPosting?.salaryPeriod,
              resume.jobPosting?.salaryCurrency,
              resume.jobPosting?.salary,
            ),
          )}
          {renderInfo("Status", currentStatus)}
          {appliedOn && renderInfo("Applied on", formatDate(appliedOn))}
          {interviewedOn &&
            renderInfo("Interviewed on", formatDate(interviewedOn))}
          {offerReceivedOn &&
            renderInfo("Received offer on", formatDate(offerReceivedOn))}
          {offerAcceptedOn &&
            renderInfo("Accepted offer on", formatDate(offerAcceptedOn))}
          {rejectedOn && renderInfo("Rejected on", formatDate(rejectedOn))}
        </div>
      </Link>
      <div className="sshrink-0 p-2 border-t border-gray-300 flex justify-center gap-2">
        <DropdownButton
          label="Edit Post"
          styleType="filter"
          containerAlignment="center"
        >
          <ActionList
            actions={[
              {
                actionName: "Edit",
                actionFn: () => console.log("Edit clicked"),
              },
            ]}
          />
        </DropdownButton>
        <DropdownButton
          label="Set Status"
          styleType="filter"
          containerAlignment="center"
        >
          <ActionList
            actions={[
              {
                actionName: "Applied",
                actionFn: () => handleStatusChange(ApplicationStatus.Applied),
              },
              {
                actionName: "Interviewing",
                actionFn: () =>
                  handleStatusChange(ApplicationStatus.Interviewed),
              },
              {
                actionName: "Offered",
                actionFn: () =>
                  handleStatusChange(ApplicationStatus.OfferReceived),
              },
              {
                actionName: "Rejected",
                actionFn: () => handleStatusChange(ApplicationStatus.Rejected),
              },
            ]}
          />
        </DropdownButton>
      </div>
    </div>
  );
};

export default ResumeCard;
