import type { ExtractedFieldValue } from "./ExtractedFieldValue";

export type ExtractedPageData = {
  sourceUrl: string;
  siteName: string;
  definitionId: number;
  definitionVersion: number;
  fields: ExtractedFieldValue[];
};
