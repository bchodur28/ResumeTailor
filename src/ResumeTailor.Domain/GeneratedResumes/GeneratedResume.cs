using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Domain.GeneratedResumes;

public class GeneratedResume : Entity
{
    public int AccountId { get; private set; }
    public int? JobApplicationId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    private readonly List<ResumeCompanySelection> _companySelections = [];
    public IReadOnlyCollection<ResumeCompanySelection> CompanySelections => _companySelections.AsReadOnly();

    private readonly List<ResumeEducationSelection> _educationSelections = [];
    public IReadOnlyCollection<ResumeEducationSelection> EducationSelections => _educationSelections.AsReadOnly();

    private readonly List<ResumeProjectSelection> _projectSelections = [];
    public IReadOnlyCollection<ResumeProjectSelection> ProjectSelections => _projectSelections.AsReadOnly();

    public ResumeAiMetaData? AiMetaData { get; private set; }

    public ResumeAiAnalysis? AiAnalysis { get; private set; }

    public GeneratedResume(int accountId, string name, int? jobApplicationId)
    {
        AccountId = accountId;
        Name = name;
        JobApplicationId = jobApplicationId;
    }

    public void Update(string name, int? jobApplicationId)
    {
        Name = name;
        JobApplicationId = jobApplicationId;
        MarkUpdated();
    }

    public ResumeCompanySelection AddCompanySelection(int companyId, int sortOrder)
    {
        var companySelection = new ResumeCompanySelection(companyId, sortOrder);
        _companySelections.Add(companySelection);
        return companySelection;
    }

    public void AddEducationSelection(int educationId, int sortOrder)
    {
        _educationSelections.Add(new ResumeEducationSelection(educationId, sortOrder));
    }

    public void AddProjectSelection(int projectId, int sortOrder)
    {
        _projectSelections.Add(new ResumeProjectSelection(projectId, sortOrder));
    }

    public void SetAiMetaData(ResumeAiMetaData aiMetaData)
    {
        AiMetaData = aiMetaData;
    }

    public void SetAiAnalysis(ResumeAiAnalysis aiSummary)
    {
        AiAnalysis = aiSummary;
    }
}
