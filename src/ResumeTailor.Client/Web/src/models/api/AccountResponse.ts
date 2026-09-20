import type { TitleResponse } from "./TitleResponse";
import type { PersonalLinkResponse } from "./PersonalLinkResponse";

export type AccountResponse = {
  id: number;
  email: string;
  displayName: string;
  city: string;
  state: string;
  country: string;
  titles: TitleResponse[];
  personalLinks: PersonalLinkResponse[];
};
