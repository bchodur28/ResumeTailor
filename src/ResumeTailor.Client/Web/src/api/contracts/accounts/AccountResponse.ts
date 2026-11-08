import type { TitleResponse } from "./TitleResponse";
import type { PersonalLinkResponse } from "./PersonalLinkResponse";

export type AccountResponse = {
  id: number;
  email: string;
  displayName: string;
  location: string;
  titles: TitleResponse[];
  personalLinks: PersonalLinkResponse[];
};
