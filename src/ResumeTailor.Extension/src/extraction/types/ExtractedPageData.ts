import type { ExtractedFieldValue } from "./ExtractedFieldValue";

export type ExtractedPageData = {
  sourceUrl: string;
  siteName: string;
  definitionId: string;
  definitionVerison: number;
  fields: ExtractedFieldValue[];
};
