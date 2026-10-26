

namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record AiResumeDataOutput(IReadOnlyCollection<AiExperienceOutput> Experiences, AiJobMatchAnalysisOutput JobMatchAnalysis, AiJobPostingDataOutput JobPostingData)
{
}
