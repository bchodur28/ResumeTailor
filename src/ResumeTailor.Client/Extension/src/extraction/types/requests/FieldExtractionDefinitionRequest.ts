import type { ExtractionType } from "../definitions/ExtractionType";
import type { JobFieldName } from "../definitions/JobFieldName";

export type FieldExtractionDefinitionRequest = {
  siteExtractionDefinitionId: number;
  fieldName: JobFieldName;
  displayLabel: string;
  extractionType: ExtractionType;
  attributeName: string | null;
  isRequired: boolean;
  sortOrder: number;
};
