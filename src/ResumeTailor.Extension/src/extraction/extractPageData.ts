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

  const values = createValueMap(fields);

  return {
    sourceUrl: normalizedUrl.toString(),
    siteName: definition.siteName,
    definitionId: definition.id,
    definitionVersion: definition.version,
    fields,
    values,
  };
};

const createValueMap = (
  fields: ExtractedFieldValue[],
): Partial<Record<JobFieldName, string>> => {
  return fields.reduce<Partial<Record<JobFieldName, string>>>(
    (values, field) => {
      if (field.value !== null) {
        values[field.fieldName] = field.value;
      }

      return values;
    },
    {},
  );
};
