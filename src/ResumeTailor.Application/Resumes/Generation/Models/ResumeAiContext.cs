namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record ResumeAiContext(string JobDescription, IReadOnlyCollection<CompanyAiContext> Companies);
