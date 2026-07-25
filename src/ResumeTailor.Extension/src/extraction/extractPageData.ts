import { extractFieldValue } from "./extractFieldValue";
import { normalizeUrl } from "./normalizeUrl";
import type { ExtractedPageData } from "./types/ExtractedPageData";
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
    sourceUrl: normalizeUrl.toString(),
    siteName: definition.siteName,
    definitionId: definition.id,
    definitionVerison: definition.version,
    fields,
  };
};
