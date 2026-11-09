import type { TitleResponse } from "./TitleResponse";
import type { PersonalLinkResponse } from "./PersonalLinkResponse";
import type { DateFormatValue } from "../../../models/common/DateFormat";

export type AccountResponse = {
  id: number;
  email: string;
  displayName: string;
  location: string;
  dateFormat: DateFormatValue;
  titles: TitleResponse[];
  personalLinks: PersonalLinkResponse[];
  summary: string | null;
};
