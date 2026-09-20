import type { Option } from "./Option";
import type { TitleForm } from "./TitleForm";
import type { PersonalLinkForm } from "./PersonalLinkForm";

export type AccountEditForm = {
  email: string;
  displayName: string;
  city: string;
  state?: Option;
  country?: Option;

  primaryTitle: TitleForm;
  additionalTitles: TitleForm[];

  personalLinks: PersonalLinkForm[];
};
