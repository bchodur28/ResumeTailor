import { Link } from "react-router-dom";
import type { ResumeSummaryResponse } from "../../../api/contracts/resumes/ResumeSummaryResponse";
import {
  ApplicationStatus,
  type ApplicationStatusValue,
} from "../../../models/resumes/ApplicationStatus";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { updateResumeApplicationTracking } from "../../../api/resumeApi";
import Message from "../../../components/ui/feedback/Message";
import DropdownButton from "../../../components/ui/buttons/DropdownButton";
import ButtonContainer from "../../../components/ui/buttons/ButtonContainer";
import DropdownActions from "../../../components/ui/dropdowns/DropdownActions";
import { useAuth0 } from "@auth0/auth0-react";

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
    month: "short",
    day: "numeric",
  }).format(value);
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
  const { getAccessTokenSilently } = useAuth0();

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

  const statusClasses = {
    interested: "interested",
    applied: "applied",
    interviewing: "interviewing",
    offerReceived: "offer-received",
    offerAccepted: "offer-accepted",
    rejected: "rejected",
  };

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
    mutationFn: async (status: ApplicationStatusValue) => {
      const token = await getAccessTokenSilently({
        authorizationParams: {
          audience: import.meta.env.VITE_AUTH0_AUDIENCE,
        },
      });
      return updateResumeApplicationTracking(resume.id, status, token);
    },
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
    <div
      className={`resume-card resume-card--${statusClasses[currentStatus ?? "interested"]}`}
    >
      <Link
        className="flex flex-col gap-2 flex-1"
        to={`/app/resumes/${resume.id}`}
      >
        <div className="flex items-center p-3 border-b border-slate-300 shadow-[0_3px_3px_-2px_rgba(0,0,0,0.15)]">
          <h1 className="text-md font-semibold text-gray-800">
            {getDisplayName(resume.name)}
          </h1>
        </div>
        {statusUpdatedSuccessfully === true && (
          <div
            onClick={(e) => {
              e.preventDefault();
              e.stopPropagation();
            }}
          >
            <Message
              type="success"
              message="Status updated successfully"
              onClose={() => setStatusUpdatedSuccessfully(null)}
            />
          </div>
        )}
        {statusUpdatedSuccessfully === false && (
          <div
            onClick={(e) => {
              e.preventDefault();
              e.stopPropagation();
            }}
          >
            <Message
              type="error"
              message="Failed to update status"
              onClose={() => setStatusUpdatedSuccessfully(null)}
            />
          </div>
        )}
        {(appliedOn ||
          interviewedOn ||
          offerReceivedOn ||
          offerAcceptedOn ||
          rejectedOn) && (
          <div className="grid grid-cols-4 px-4">
            {appliedOn && (
              <div>
                <div className="applied-color h-2"></div>
                <span className="text-xs font-semibold text-gray-700">
                  {`Applied: ${formatDate(appliedOn)}`}
                </span>
              </div>
            )}
            {interviewedOn && (
              <div>
                <div className="interviewed-color h-2"></div>
                <span className="text-xs font-semibold text-gray-700">
                  {`Interviewed: ${formatDate(interviewedOn)}`}
                </span>
              </div>
            )}

            {offerReceivedOn && (
              <div>
                <div className="offer-received-color h-2"></div>
                <span className="text-xs font-semibold text-gray-700">
                  {`Offer Received: ${formatDate(offerReceivedOn)}`}
                </span>
              </div>
            )}
            {/* <div className="offer-received-color h-4"></div> */}
            {rejectedOn && (
              <div>
                <div className="rejected-color h-2"></div>
                <span className="text-xs font-semibold text-gray-700">
                  {`Rejected: ${formatDate(rejectedOn)}`}
                </span>
              </div>
            )}
            {/* <div className="bg-red-200 h-4"></div> */}
            {/* {appliedOn && (
              <img
                src={applicationIcon}
                alt="Applied"
                className="h-16 w-16 object-contain"
              />
            )}

            {interviewedOn && (
              <img
                src={interviewingIcon}
                alt="Interviewing"
                className="h-16 w-16 object-contain"
              />
            )}

            {offerReceivedOn && (
              <img
                src={handShakeIcon}
                alt="Offer Received"
                className="h-16 w-16 object-contain"
              />
            )}

            {rejectedOn && (
              <img
                src={rejectedIcon}
                alt="Rejected"
                className="h-16 w-16 object-contain"
              />
            )} */}
          </div>
        )}

        <div className="flex-1 px-3 pb-4">
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
          {/* {appliedOn && renderInfo("Applied on", formatDate(appliedOn))}
          {interviewedOn &&
            renderInfo("Interviewed on", formatDate(interviewedOn))}
          {offerReceivedOn &&
            renderInfo("Received offer on", formatDate(offerReceivedOn))}
          {offerAcceptedOn &&
            renderInfo("Accepted offer on", formatDate(offerAcceptedOn))}
          {rejectedOn && renderInfo("Rejected on", formatDate(rejectedOn))} */}
        </div>
      </Link>
      <div className="shrink-0 p-2 border-t border-slate-300 flex justify-center gap-2 shadow-[0_-3px_3px_-2px_rgba(0,0,0,0.15)]">
        <ButtonContainer align="center" isRelative={true}>
          <DropdownButton label="Set Status" styleType="plain" closeOnClick>
            <DropdownActions
              actions={[
                {
                  label: "Applied",
                  onClick: () => handleStatusChange(ApplicationStatus.Applied),
                },
                {
                  label: "Interviewing",
                  onClick: () =>
                    handleStatusChange(ApplicationStatus.Interviewed),
                },
                {
                  label: "Offered",
                  onClick: () =>
                    handleStatusChange(ApplicationStatus.OfferReceived),
                },
                {
                  label: "Rejected",
                  onClick: () => handleStatusChange(ApplicationStatus.Rejected),
                },
              ]}
            />
          </DropdownButton>
        </ButtonContainer>
      </div>
    </div>
  );
};

export default ResumeCard;
