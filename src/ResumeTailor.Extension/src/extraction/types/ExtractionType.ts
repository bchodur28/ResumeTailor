export const extractionTypes = ["text", "html", "attribute"] as const;

export type ExtractionType = (typeof extractionTypes)[number];
