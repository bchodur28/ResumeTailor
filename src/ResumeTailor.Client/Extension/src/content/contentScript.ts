import { extractPageData } from "../extraction/extractPageData";
import type { SiteExtractionDefinition } from "../extraction/types/SiteExtractionDefinition";

type ExtractedPageMessage = {
  type: "EXTRACT_PAGE";
  definition: SiteExtractionDefinition;
};

chrome.runtime.onMessage.addListener(
  (message: ExtractedPageMessage, _sender, sendReponse) => {
    if (message.type !== "EXTRACT_PAGE") {
      return;
    }

    try {
      const extractedPageData = extractPageData(
        message.definition,
        document,
        window.location.href,
      );

      sendReponse({
        success: true,
        data: extractedPageData,
      });
    } catch (error) {
      sendReponse({
        success: false,
        error:
          error instanceof Error ? error.message : "Page extraction failed.",
      });
    }
  },
);
