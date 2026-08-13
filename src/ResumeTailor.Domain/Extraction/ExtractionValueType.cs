using System.Text.Json.Serialization;

namespace ResumeTailor.Domain.Extraction;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExtractionValueType
{
    ElementText = 1,
    ElementHtml = 2,
    Attribute = 3,
    TextMatch = 4
}
