namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record ResumeAiInput(string JobDescription, IReadOnlyCollection<LibraryExperienceAiInput> Experiences, string? Summary, SummaryGenerationMode SummaryMode);
