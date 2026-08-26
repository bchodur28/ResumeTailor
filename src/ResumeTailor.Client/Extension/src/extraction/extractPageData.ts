import { extractFieldValue } from "./extractFieldValue";
import { normalizeUrl } from "./normalizeUrl";
import type { ExtractedPageData } from "./types/results/ExtractedPageData";
import type { SiteExtractionDefinition } from "./types/definitions/SiteExtractionDefinition";

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
