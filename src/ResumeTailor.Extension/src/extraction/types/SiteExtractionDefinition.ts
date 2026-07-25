import type { FieldExtractionDefinition } from "./FieldExtractionDefinition";

export type SiteExtractionDefinition = {
  id: string;
  siteName: string;
  hostnamePatterns: string[];
  pathPatterns: string[];
  fields: FieldExtractionDefinition[];
  version: number;
  enabled: boolean;
};
