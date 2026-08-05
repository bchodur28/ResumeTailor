using System.Text.Json.Serialization;

namespace ResumeTailor.Domain.Extraction;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExtractionValueType
{
    Text = 1,
    Html = 2,
    Attribute = 3
}
