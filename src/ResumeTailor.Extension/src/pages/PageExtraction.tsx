import { fetchSiteExtractionDefintion } from "../api/extraction";
import type { ExtractedPageData } from "../extraction/types/ExtractedPageData";
import { useState } from "react";

const PageExtraction = () => {
  const [isExtracting, setIsExtracting] = useState(false);
  const [extractedPageData, setExtractedPageData] =
    useState<ExtractedPageData | null>(null);

  const handleExtract = async () => {
    setIsExtracting(true);
    try {
      const [tab] = await chrome.tabs.query({
        active: true,
        currentWindow: true,
      });

      if (tab.id === undefined || !tab.url) {
        throw new Error("The active tab could not be accessed.");
      }

      const currentUrl = new URL(tab.url);
      const definition = await fetchSiteExtractionDefintion(currentUrl);

      const pageExtractionResult = await chrome.tabs.sendMessage(tab.id, {
        type: "EXTRACT_PAGE",
        definition,
      });

      if (!pageExtractionResult.success) {
        throw new Error(pageExtractionResult.error);
      }

      setExtractedPageData(pageExtractionResult.data);
    } catch (error) {
      console.log("Extraction failed: ", error);
    } finally {
      setIsExtracting(false);
    }
  };

  return (
    <main style={{ padding: "1.5rem" }}>
      <h1>Resume Tailor</h1>
      <p>
        Open a job listing and click generate resume to generated a resume based
        off of your own curated bullets.
      </p>

      <div className="btn-container">
        <button className="btn" type="button" onClick={handleExtract}>
          {isExtracting ? "Extracting..." : "Extract Job Details"}
        </button>
      </div>

      {extractedPageData && (
        <ul>
          {extractedPageData.fields.map((field) => (
            <li key={field.fieldName}>
              <strong>{field.displayLabel}:</strong>
              {field.value ?? "Value wasn't found"}
            </li>
          ))}
        </ul>
      )}
    </main>
  );
};

export default PageExtraction;
