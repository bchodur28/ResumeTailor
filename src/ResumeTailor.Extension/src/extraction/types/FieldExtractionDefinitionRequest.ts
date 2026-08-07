import type { ExtractionType } from "./ExtractionType";
import type { JobFieldName } from "./JobFieldName";

export type FieldExtractionDefinitionRequest = {
  siteExtractionDefinitionId: number;
  fieldName: JobFieldName;
  displayLabel: string;
  extractionType: ExtractionType;
  attributeName: string | null;
  isRequired: boolean;
  sortOrder: number;
};
