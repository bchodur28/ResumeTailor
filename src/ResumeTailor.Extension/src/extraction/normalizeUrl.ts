const trackingparameterPrefixes = ["utm_"];

const trackingParameters = new Set(["trackingId", "refId", "trk", "lipi"]);

export const normalizeUrl = (rawUrl: string): URL => {
  const url = new URL(rawUrl);

  url.hostname = url.hostname.toLowerCase();

  removeTrackingParameters(url);

  if (url.pathname.length > 1) {
    url.pathname = url.pathname.replace(/\/+$/, "");
  }

  url.hash = "";

  return url;
};

const removeTrackingParameters = (url: URL): void => {
  const parametersToDelete: string[] = [];

  for (const parameterName of url.searchParams.keys()) {
    const isTrackingParameter =
      trackingParameters.has(parameterName) ||
      trackingparameterPrefixes.some((prefix) =>
        parameterName.toLowerCase().startsWith(prefix),
      );

    if (isTrackingParameter) {
      parametersToDelete.push(parameterName);
    }
  }

  for (const parameterName of parametersToDelete) {
    url.searchParams.delete(parameterName);
  }
};
