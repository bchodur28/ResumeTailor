namespace ResumeTailor.Application.Resumes.Models;

public sealed record GeneratedCompany(
    string Name,
    string Position,
    string WorkingStatus,
    string Location,
    List<string> Bullets
    );
