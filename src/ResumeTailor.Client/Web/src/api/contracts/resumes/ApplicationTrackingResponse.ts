export type ApplicationTrackingResponse = {
  status: string;
  applied: string | null;
  interviewed: string | null;
  offerReceived: string | null;
  offerAccepted: string | null;
  rejected: string | null;
};
