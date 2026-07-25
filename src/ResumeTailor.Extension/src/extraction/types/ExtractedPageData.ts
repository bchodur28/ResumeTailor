import type { ExtractedFieldValue } from "./ExtractedFieldValue";
import type { JobFieldName } from "./JobFieldName";

export type ExtractedPageData = {
  sourceUrl: string;
  siteName: string;
  definitionId: string;
  definitionVersion: number;
  fields: ExtractedFieldValue[];
  values: Partial<Record<JobFieldName, string>>;
};
