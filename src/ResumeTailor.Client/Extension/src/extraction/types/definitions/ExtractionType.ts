export const extractionTypes = [
  "text",
  "html",
  "attribute",
  "textMatch",
] as const;

export type ExtractionType = (typeof extractionTypes)[number];
