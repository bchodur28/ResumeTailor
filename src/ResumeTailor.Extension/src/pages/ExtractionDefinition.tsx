import { useQuery } from "@tanstack/react-query";
import { fetchSiteExtractionDefintion } from "../api/extraction";
import CreateFieldForm from "../components/forms/CreateFieldForm";
import UpdateFieldForm from "../components/forms/UpdateFieldForm";

const ExtractionDefinition = () => {
  const {
    data: siteDefinition,
    isLoading: isSiteDefinitionLoading,
    error: siteDefinitionError,
  } = useQuery({
    queryKey: ["siteExtractionDefinition"],
    queryFn: async () => {
      const [tab] = await chrome.tabs.query({
        active: true,
        currentWindow: true,
      });

      if (tab.id === undefined || !tab.url) {
        throw new Error("The active tab could not be accessed.");
      }

      const currentUrl = new URL(tab.url);

      const definition = fetchSiteExtractionDefintion(currentUrl);
      console.log(definition);
      return definition;
    },
  });
  return (
    <div>
      {isSiteDefinitionLoading && <p>Loading site definition...</p>}
      {siteDefinitionError && (
        <p>
          Error:{" "}
          {siteDefinitionError instanceof Error
            ? siteDefinitionError.message
            : "An unknown error occurred."}
        </p>
      )}
      {siteDefinition && (
        <>
          <h2>Website: {siteDefinition.siteName}</h2>
          <h3>Hostname: {siteDefinition.hostname}</h3>
          <h4>Id: {siteDefinition.id}</h4>
          <h4>Path Pattern: {siteDefinition.pathPattern}</h4>
          <h4>Version: {siteDefinition.version}</h4>
          <CreateFieldForm siteId={siteDefinition.id} />
          {siteDefinition.fields.map((field) => {
            return (
              <UpdateFieldForm
                key={field.id}
                id={field.id}
                siteId={siteDefinition.id}
                fieldName={field.fieldName}
                displayLabel={field.displayLabel}
                extractionType={field.extractionType}
                attributeName={field.attributeName}
                isRequired={field.isRequired}
                sortOrder={field.sortOrder}
                selectors={field.selectors}
              />
            );
          })}
        </>
      )}
    </div>
  );
};

export default ExtractionDefinition;
