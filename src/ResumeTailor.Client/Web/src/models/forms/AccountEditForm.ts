import type { TitleForm } from "./TitleForm";
import type { PersonalLinkForm } from "./PersonalLinkForm";

export type AccountEditForm = {
  email: string;
  displayName: string;
  location: string;

  primaryTitle: TitleForm;
  additionalTitles: TitleForm[];

  personalLinks: PersonalLinkForm[];
};
