using ResumeTailor.Application.Common.Models;
using ResumeTailor.Domain.Resumes.Content;


namespace ResumeTailor.Application.Resumes.Generation.Models;

public record ResumeFinalizationPayload(
    AiResult<AiResumeDataOutput> AiResult,
    IReadOnlyCollection<ResolvedResumeExperience> ResolvedExperiences,
    string? ResumeSummary,
    string? AiSummary
    );
