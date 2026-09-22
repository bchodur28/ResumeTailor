using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeRequest(
    int AccountId,
    int? JobApplicationId,
    string Name,
    IReadOnlyCollection<ResumeCompanySelectionRequest> Companies,
    IReadOnlyCollection<ResumeEducationSelectionRequest> Education,
    IReadOnlyCollection<ResumeProjectSelectionRequest> Projects,
    ResumeAiAnalysisRequest AiAnalysis,
    ResumeAiMetaDataRequest AiMetaData);
