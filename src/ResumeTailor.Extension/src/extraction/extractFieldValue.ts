import type { ExtractedFieldValue } from "./types/results/ExtractedFieldValue";
import type { FieldExtractionDefinition } from "./types/definitions/FieldExtractionDefinition";

export const extractFieldValue = (
  definition: FieldExtractionDefinition,
  document: Document,
): ExtractedFieldValue => {
  for (const selector of definition.selectors) {
    const element =
      definition.extractionType === "textMatch"
        ? document.body
        : findElement(document, selector.selector);

    if (!element) {
      continue;
    }

    const value = readElementValue(element, definition, selector.selector);

    if (!value) {
      continue;
    }

    return {
      fieldName: definition.fieldName,
      displayLabel: definition.displayLabel,
      value,
      matchedSelector: selector.selector,
      required: definition.isRequired,
    };
  }

  return {
    fieldName: definition.fieldName,
    displayLabel: definition.displayLabel,
    value: null,
    matchedSelector: null,
    required: definition.isRequired,
  };
};

const findElement = (
  document: Document,
  selector: string,
): HTMLElement | null => {
  try {
    return document.querySelector(selector);
  } catch {
    console.log(`Invalid extraction selector: ${selector}`);
    return null;
  }
};

const readElementValue = (
  element: Element,
  definition: FieldExtractionDefinition,
  startsWith?: string,
): string | null => {
  switch (definition.extractionType) {
    case "text":
      return normalizeText(element.textContent);
    case "html":
      return normalizeHtml(element.innerHTML);
    case "attribute":
      return readAttributeValue(element, definition.attributeName);
    case "textMatch":
      return startsWith
        ? findTextMatch(element as HTMLElement, startsWith)
        : null;
  }
};

const findTextMatch = (
  element: HTMLElement,
  startsWith: string,
): string | null => {
  const words = element.innerText.split(/\s+/) || [];

  return words.find((word) => word.startsWith(startsWith)) || null;
};

const normalizeText = (value: string | null): string | null => {
  if (!value) {
    return null;
  }

  const normalized = value
    .replace(/\u00a0/g, " ")
    .replace(/\s+/g, " ")
    .trim();

  return normalized.length > 0 ? normalized : null;
};

const normalizeHtml = (value: string): string | null => {
  const normalized = value.trim();

  return normalized.length > 0 ? normalized : null;
};

const readAttributeValue = (
  element: Element,
  attributeName?: string | null,
): string | null => {
  if (!attributeName) {
    return null;
  }

  return normalizeText(element.getAttribute(attributeName));
};
