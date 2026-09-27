using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Domain.GeneratedResumes;

public class Resume : Entity
{
    public int AccountId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public AiScoreStaleness AiScoreStaleness { get; private set; } = AiScoreStaleness.None;

    private readonly List<ResumeCompanySelection> _companySelections = [];
    public IReadOnlyCollection<ResumeCompanySelection> CompanySelections => _companySelections.AsReadOnly();

    private readonly List<ResumeEducationSelection> _educationSelections = [];
    public IReadOnlyCollection<ResumeEducationSelection> EducationSelections => _educationSelections.AsReadOnly();

    private readonly List<ResumeProjectSelection> _projectSelections = [];
    public IReadOnlyCollection<ResumeProjectSelection> ProjectSelections => _projectSelections.AsReadOnly();

    public ResumeAiAnalysis? AiAnalysis { get; private set; }

    public ResumeAiMetaData? AiMetaData { get; private set; }

    public ResumeJobPosting? JobPosting { get; private set; }

    public ResumeApplicationTracking? ApplicationTracking { get; private set; }

    public Resume(int accountId, string name)
    {
        AccountId = accountId;
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
        MarkUpdated();
    }

    public void MarkAiScoreStale(AiScoreStaleness staleness)
    {
        if (staleness > AiScoreStaleness)
        {
            AiScoreStaleness = staleness;
        }
    }

    public ResumeCompanySelection AddCompanySelection(int companyId, int sortOrder)
    {
        var companySelection = new ResumeCompanySelection(companyId, sortOrder);
        _companySelections.Add(companySelection);
        return companySelection;
    }

    public void RemoveCompanySelection(ResumeCompanySelection companySelection)
    {
        _companySelections.Remove(companySelection);
    }

    public void AddEducationSelection(int educationId, int sortOrder)
    {
        _educationSelections.Add(new ResumeEducationSelection(educationId, sortOrder));
    }

    public void RemoveEducationSelection(ResumeEducationSelection educationSelection)
    {
        _educationSelections.Remove(educationSelection);
    }

    public void AddProjectSelection(int projectId, int sortOrder)
    {
        _projectSelections.Add(new ResumeProjectSelection(projectId, sortOrder));
    }

    public void RemoveProjectSelection(ResumeProjectSelection projectSelection)
    {    
        _projectSelections.Remove(projectSelection);
    }

    public void SetAiMetaData(ResumeAiMetaData aiMetaData)
    {
        AiMetaData = aiMetaData;
    }

    public void SetAiAnalysis(ResumeAiAnalysis aiSummary)
    {
        AiAnalysis = aiSummary;
    }

    public void SetJobPosting(ResumeJobPosting jobPosting)
    {
        JobPosting = jobPosting;
    }

    public void SetApplicationTracking(ResumeApplicationTracking applicationTracking)
    {
        ApplicationTracking = applicationTracking;
    }
}
