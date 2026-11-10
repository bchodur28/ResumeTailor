using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record GeneratedResumeRequest(string JobDescription, SummaryGenerationMode SummaryMode);
