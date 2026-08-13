import type { ExtractionType } from "./ExtractionType";
import type { FieldSelector } from "./FieldSelector";
import type { JobFieldName } from "./JobFieldName";

export type FieldExtractionDefinition = {
  id: number;
  siteId: number;
  fieldName: JobFieldName;
  displayLabel: string;
  extractionType: ExtractionType;
  attributeName: string | null;
  isRequired: boolean;
  sortOrder: number;
  selectors: FieldSelector[];
};
