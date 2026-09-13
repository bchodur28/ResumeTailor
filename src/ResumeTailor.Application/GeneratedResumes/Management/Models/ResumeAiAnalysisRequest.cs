namespace ResumeTailor.Application.GeneratedResumes.Management.Models;

public sealed record ResumeAiAnalysisRequest(
    string Summary,
    int Score,
    IReadOnlyCollection<ResumeAiInsightRequest> Strengths,
    IReadOnlyCollection<ResumeAiInsightRequest> Weaknesses
    );
