import { extractFieldValue } from "./extractFieldValue";
import { normalizeUrl } from "./normalizeUrl";
import type { ExtractedFieldValue } from "./types/ExtractedFieldValue";
import type { ExtractedPageData } from "./types/ExtractedPageData";
import type { JobFieldName } from "./types/JobFieldName";
import type { SiteExtractionDefinition } from "./types/SiteExtractionDefinition";

export const extractPageData = (
  definition: SiteExtractionDefinition,
  document: Document,
  currentUrl: string,
): ExtractedPageData => {
  const normalizedUrl = normalizeUrl(currentUrl);

  const fields = definition.fields.map((fieldDefinition) =>
    extractFieldValue(fieldDefinition, document),
  );

  return {
    sourceUrl: normalizedUrl.toString(),
    siteName: definition.siteName,
    definitionId: definition.id,
    definitionVersion: definition.version,
    fields,
  };
};
