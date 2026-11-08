import type { PersonalLinkRequest } from "./PersonalLinkRequest";
import type { TitleRequest } from "./TitleRequest";

export type AccountRequest = {
  email: string;
  displayName: string;
  location: string;
  personalLinks: PersonalLinkRequest[];
  titles: TitleRequest[];
};
