import type { JobFieldName } from "./JobFieldName";

export type ExtractedFieldValue = {
  fieldName: JobFieldName;
  displayLabel: string;
  value: string | null;
  matchedSelector: string | null;
  required: boolean;
};
