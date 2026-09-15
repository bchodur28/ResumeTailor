import type { Option } from "./Option";

export type AccountSetupForm = {
  email: string;
  displayName: string;
  city: string;
  state?: Option;
  country?: Option;

  primaryWebsiteDisplay: string;
  primaryWebsiteUrl: string;

  primaryTitle: string;

  additionalTitles: {
    value: string;
  }[];

  additionalWebsites: {
    display: string;
    url: string;
  }[];
};
