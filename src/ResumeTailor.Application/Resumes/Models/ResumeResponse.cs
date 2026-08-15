namespace ResumeTailor.Application.Resumes.Models;

public record ResumeResponse(
    int Id,
    string PersonName,
    string Profession,
    string Email,
    string PhoneNumber,
    string? College,
    string? Degree,
    string? Major,
    string? CollegeStatus,
    string? PersonalSite1,
    string? PersonalSite2,
    string? PersonalSite3,
    IReadOnlyCollection<string> Skills,
    IReadOnlyCollection<CompanyResponse> Companies
);

