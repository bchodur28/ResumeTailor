namespace ResumeTailor.Application.GeneratedResumes.Management.Models;

public sealed record SaveGeneratedResumeRequest(
    int AccountId,
    int? JobApplicationId,
    string Name,
    IReadOnlyCollection<ResumeCompanySelectionRequest> Companies,
    IReadOnlyCollection<ResumeEducationSelectionRequest> Education,
    IReadOnlyCollection<ResumeProjectSelectionRequest> Projects,
    ResumeAiAnalysisRequest AiAnalysis,
    ResumeAiMetaDataRequest AiMetaData);
