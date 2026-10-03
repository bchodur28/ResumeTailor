export const ApplicationStatus = {
  Interested: "interested",
  Applied: "applied",
  Interviewed: "interviewing",
  OfferReceived: "offerReceived",
  OfferAccepted: "offerAccepted",
  Rejected: "rejected",
} as const;

export type ApplicationStatusValue =
  (typeof ApplicationStatus)[keyof typeof ApplicationStatus];
