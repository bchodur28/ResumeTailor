namespace ResumeTailor.Application.Resumes.Models;

public record ResumeRequest(
    string PersonName,
    string Profession,
    string Email,
    string PhoneNumber,
    string? College = null,
    string? Degree = null,
    string? Major = null,
    string? CollegeStatus = null,
    string? PersonalSite1 = null,
    string? PersonalSite2 = null,
    string? PersonalSite3 = null
);

