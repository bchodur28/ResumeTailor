import type { PersonalLinkRequest } from "./PersonalLinkRequest";
import type { TitleRequest } from "./TitleRequest";

export type AccountRequest = {
  email: string;
  displayName: string;
  city: string;
  state: string;
  country: string;
  personalLinks: PersonalLinkRequest[];
  titles: TitleRequest[];
};
