using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.Content;
using ResumeTailor.Domain.Resumes.JobPositing;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Domain.GeneratedResumes;

public class Resume : Entity
{
    public int AccountId { get; private set; }
    public Account Account { get; private set; }

    public string Name { get; private set; } = null!;

    public int? AiScore { get; private set; }

    public AiScoreStaleness AiScoreStaleness { get; private set; } = AiScoreStaleness.None;

    private readonly List<ResumeEducationSelection> _educationSelections = [];
    public IReadOnlyCollection<ResumeEducationSelection> EducationSelections => _educationSelections.AsReadOnly();

    private readonly List<ResumeExperience> _experiences = [];
    public IReadOnlyCollection<ResumeExperience> Experiences => _experiences.AsReadOnly();

    public ResumeAiAnalysis? AiAnalysis { get; private set; }

    public ResumeAiMetaData AiMetaData { get; private set; } = null!;

    public ResumeJobPosting JobPosting { get; private set; } = null!;

    public ResumeApplicationTracking ApplicationTracking { get; private set; } = null!;

    public ResumeAppearance Appearance { get; private set; } = null!;

    private Resume()
    {
        // EF Core
    }

    public Resume(
        int accountId,
        string name,
        int? aiScore,
        ResumeAiMetaData aiMetaData,
        ResumeJobPosting jobPosting)
    {
        AccountId = accountId;
        Name = name;
        AiScore = aiScore;
        AiMetaData = aiMetaData;
        JobPosting = jobPosting;
        ApplicationTracking = ResumeApplicationTracking.CreateDefault();
        Appearance = ResumeAppearance.CreateDefault();
        AiScoreStaleness = AiScoreStaleness.None;
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

    public ResumeExperience AddExperience(int libraryExperienceId, int? librarySummaryId, int sortOrder)
    {
        var experience = new ResumeExperience(libraryExperienceId, sortOrder);
        _experiences.Add(experience);
        return experience;
    }

    public void RemoveExperience(ResumeExperience experience)
    {
        _experiences.Remove(experience);
    }

    public void AddEducationSelection(int educationId, int sortOrder)
    {
        _educationSelections.Add(new ResumeEducationSelection(educationId, sortOrder));
    }

    public void RemoveEducationSelection(ResumeEducationSelection educationSelection)
    {
        _educationSelections.Remove(educationSelection);
    }

    public void SetAiAnalysis(ResumeAiAnalysis aiSummary)
    {
        AiAnalysis = aiSummary;
    }

    public void SetApplicationTracking(ResumeApplicationTracking applicationTracking)
    {
        ApplicationTracking = applicationTracking;
    }

    public void SetAppearance(ResumeAppearance appearance)
    {
        Appearance = appearance;
    }
}
