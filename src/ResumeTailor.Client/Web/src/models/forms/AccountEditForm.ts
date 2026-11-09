import type { TitleForm } from "./TitleForm";
import type { PersonalLinkForm } from "./PersonalLinkForm";
import type { DateFormatValue } from "../common/DateFormat";

export type AccountEditForm = {
  email: string;
  displayName: string;
  location: string;
  dateFormat: DateFormatValue;

  primaryTitle: TitleForm;
  additionalTitles: TitleForm[];

  personalLinks: PersonalLinkForm[];
  summary: string | null;
};
