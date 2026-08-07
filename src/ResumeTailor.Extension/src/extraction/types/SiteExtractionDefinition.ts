import type { FieldExtractionDefinition } from "./FieldExtractionDefinition";

export type SiteExtractionDefinition = {
  id: number;
  siteName: string;
  hostname: string[];
  pathPattern: string[];
  fields: FieldExtractionDefinition[];
  version: number;
  enabled: boolean;
};
