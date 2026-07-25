import type { ExtractionType } from "./ExtractionType";
import type { JobFieldName } from "./JobFieldName";

export type FieldExtractionDefinition = {
  fieldName: JobFieldName;
  displayLabel: string;
  selectors: string[];
  extractionType: ExtractionType;
  required: boolean;
};
