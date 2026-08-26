import type { ExtractionType } from "./ExtractionType";
import type { FieldPattern } from "./FieldPattern";
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
  patterns: FieldPattern[];
};
