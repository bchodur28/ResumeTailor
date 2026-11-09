import type { PersonalLinkRequest } from "./PersonalLinkRequest";
import type { TitleRequest } from "./TitleRequest";
import type { DateFormatValue } from "../../../models/common/DateFormat";

export type AccountRequest = {
  email: string;
  displayName: string;
  location: string;
  dateFormat: DateFormatValue;
  personalLinks: PersonalLinkRequest[];
  titles: TitleRequest[];
  summary: string | null;
};
