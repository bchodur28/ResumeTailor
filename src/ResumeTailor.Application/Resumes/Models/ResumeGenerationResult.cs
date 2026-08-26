namespace ResumeTailor.Application.Resumes.Models;

public sealed record ResumeGenerationResult(CompanyGeneratedResume Resume, AiUsage Usage);
