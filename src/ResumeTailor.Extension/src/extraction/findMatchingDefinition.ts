import type { SiteExtractionDefinition } from "./types/SiteExtractionDefinition";

export const findMatchingDefinition = (
  currentUrl: string,
  definitions: SiteExtractionDefinition[],
): SiteExtractionDefinition | undefined => {
  const url = new URL(currentUrl);

  return definitions.find((definitions) => {
    const hostnameMatches = definitions.hostnamePatterns.some(
      (hostname) =>
        url.hostname === hostname || url.hostname.endsWith(`.${hostname}`),
    );

    const pathMatches = definitions.pathPatterns.some((pattern) =>
      matchesPathPattern(url.pathname, pattern),
    );

    return definitions.enabled && hostnameMatches && pathMatches;
  });
};

const matchesPathPattern = (pathname: string, pattern: string): boolean => {
  const escapedPattern = pattern
    .replace(/[.+?^${}()|[\]\\]/g, "\\$&")
    .replaceAll("*", ".*");

  return new RegExp(`^${escapedPattern}$`, "i").test(pathname);
};
