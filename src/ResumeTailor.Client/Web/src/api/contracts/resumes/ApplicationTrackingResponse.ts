import type { ApplicationStatusValue } from "../../../models/resumes/ApplicationStatus";

export type ApplicationTrackingResponse = {
  status: ApplicationStatusValue;
  applied: string | null;
  interviewed: string | null;
  offerReceived: string | null;
  offerAccepted: string | null;
  rejected: string | null;
};
