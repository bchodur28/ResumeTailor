using System.Text.Json.Serialization;

namespace ResumeTailor.Domain.Extraction;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum JobFieldName
{
    JobTitle = 1,
    CompanyName = 2,
    Location = 3,
    Description = 4,
    Salary = 5,
    EmploymentType = 6,
    PostedDate = 7
}
