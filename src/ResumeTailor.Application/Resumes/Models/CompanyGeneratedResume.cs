namespace ResumeTailor.Application.Resumes.Models;

public sealed record CompanyGeneratedResume(
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
    List<string> Skills,
    List<GeneratedCompany> Companies);
