import type { ExtractionType } from "./ExtractionType";
import type { FieldSelector } from "./FieldSelector";
import type { JobFieldName } from "./JobFieldName";

export type FieldExtractionDefinition = {
  fieldName: JobFieldName;
  displayLabel: string;
  selectors: FieldSelector[];
  extractionType: ExtractionType;
  attributeName?: string;
  required: boolean;
};
