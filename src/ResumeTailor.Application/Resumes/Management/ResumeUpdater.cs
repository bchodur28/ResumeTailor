using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.Contracts.Resume.Selection;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Application.Resumes.Management.Interfaces;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes.Management;

public sealed class ResumeUpdater : IResumeUpdater
{
    public void Update(Resume resume, UpdateResumeRequest request)
    {
        resume.Update(request.Name);
        UpdateAppearance(request, resume);

        UpdateCompanySelections(resume, request.Companies);
        UpdateEducationSelections(resume, request.Education);
        UpdateProjectSelections(resume, request.Projects);
    }

    private static void UpdateAppearance(UpdateResumeRequest request, Resume existingResume)
    {
        existingResume.Appearance?.Update(
                    titleFontSize: request.Appearance.TitleFontSize,
                    sectionHeaderFontSize: request.Appearance.SectionHeaderFontSize,
                    mainBodyFontSize: request.Appearance.MainBodyFontSize,
                    fontFamily: request.Appearance.FontFamily,
                    fontColor: request.Appearance.FontColor,
                    topHeaderAlignment: request.Appearance.TopHeaderAlignment);
    }


    private static void UpdateCompanySelections(Resume resume, IReadOnlyCollection<CompanySelectionRequest> companySelectionRequests)
    {

        var requestedCompanyIds = companySelectionRequests
            .Select(r => r.CompanyId)
            .ToHashSet();

        var companySelectionsToDelete = resume.CompanySelections
            .Where(s => !requestedCompanyIds.Contains(s.CompanyId))
            .ToList();

        foreach (var selection in companySelectionsToDelete)
        {
            resume.MarkAiScoreStale(AiScoreStaleness.High);
            resume.RemoveCompanySelection(selection);
        }

        foreach (var companyRequest in companySelectionRequests)
        {
            if (companyRequest.Id.HasValue)
            {
                var existingCompanySelection = resume.CompanySelections.FirstOrDefault(s => s.Id == companyRequest.Id.Value)
                    ?? throw new NotFoundException($"Company selection with ID {companyRequest.Id.Value} was not found while updating.");

                if (existingCompanySelection.CompanyId != companyRequest.CompanyId)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                } else if (existingCompanySelection.SortOrder != companyRequest.SortOrder)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.Medium);
                }

                existingCompanySelection.Update(companyRequest.CompanyId, companyRequest.SortOrder);

                var requestedBulletIds = companyRequest.Bullets
                    .Where(b => b.Id.HasValue)
                    .Select(b => b.Id!.Value)
                    .ToHashSet();

                var bulletsToDelete = existingCompanySelection.Bullets
                    .Where(b => !requestedBulletIds.Contains(b.Id))
                    .ToList();

                foreach (var bullet in bulletsToDelete)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                    existingCompanySelection.RemoveResumeBullet(bullet);
                }

                foreach (var bulletRequest in companyRequest.Bullets)
                {
                    if (bulletRequest.Id.HasValue)
                    {
                        var existingBullet = existingCompanySelection.Bullets.FirstOrDefault(b => b.Id == bulletRequest.Id.Value)
                            ?? throw new NotFoundException($"Bullet with ID {bulletRequest.Id} was not found while updating.");

                        var staleness = GetBulletStaleness(existingBullet, bulletRequest);
                        resume.MarkAiScoreStale(staleness);

                        existingBullet.Update(bulletRequest.BulletId, bulletRequest.Value, bulletRequest.AlternativeValue, bulletRequest.SortOrder);
                    } else
                    {
                        resume.MarkAiScoreStale(AiScoreStaleness.High);

                        existingCompanySelection.AddResumeBullet(
                            bulletRequest.BulletId,
                            bulletRequest.Value,
                            bulletRequest.AlternativeValue,
                            bulletRequest.SortOrder);
                    }

                }
            } else
            {
                resume.MarkAiScoreStale(AiScoreStaleness.High);

                var newCompanySelection = resume.AddCompanySelection(companyRequest.CompanyId, companyRequest.SortOrder);

                foreach (var bulletRequest in companyRequest.Bullets)
                {
                    newCompanySelection.AddResumeBullet(
                        bulletRequest.BulletId,
                        bulletRequest.Value,
                        bulletRequest.AlternativeValue,
                        bulletRequest.SortOrder);
                }
            }
        }
    }

    private static void UpdateEducationSelections(Resume resume, IReadOnlyCollection<EducationSelectionRequest> educationSelectionRequests)
    {
        var requestedEducationIds = educationSelectionRequests
            .Select(r => r.EducationId)
            .ToHashSet();

        var educationSelectionsToDelete = resume.EducationSelections
            .Where(s => !requestedEducationIds.Contains(s.EducationId))
            .ToList();

        foreach (var selection in educationSelectionsToDelete)
        {
            resume.RemoveEducationSelection(selection);
        }

        foreach (var educationRequest in educationSelectionRequests)
        {
            if (educationRequest.Id.HasValue)
            {
                var existingEducationSelection = resume.EducationSelections.FirstOrDefault(s => s.Id == educationRequest.Id)
                    ?? throw new NotFoundException($"Education selection with ID {educationRequest.EducationId} was not found while updating.");

                existingEducationSelection.Update(educationRequest.EducationId, educationRequest.SortOrder);
            } else
            {
                resume.AddEducationSelection(educationRequest.EducationId, educationRequest.SortOrder);
            }
        }
    }

    private static void UpdateProjectSelections(Resume resume, IReadOnlyCollection<ProjectSelectionRequest> projectSelectionRequests)
    {
        var requestedProjectIds = projectSelectionRequests
            .Select(r => r.ProjectId)
            .ToHashSet();

        var projectSelectionsToDelete = resume.ProjectSelections
            .Where(s => !requestedProjectIds.Contains(s.ProjectId))
            .ToList();

        foreach (var selection in projectSelectionsToDelete)
        {
            resume.RemoveProjectSelection(selection);
        }

        foreach (var projectRequest in projectSelectionRequests)
        {
            if (projectRequest.Id.HasValue)
            {
                var existingProjectSelection = resume.ProjectSelections.FirstOrDefault(s => s.Id == projectRequest.Id)
                    ?? throw new NotFoundException($"Project selection with ID {projectRequest.ProjectId} was not found while updating.");

                existingProjectSelection.Update(projectRequest.ProjectId, projectRequest.SortOrder);
            } else
            {
                resume.AddProjectSelection(projectRequest.ProjectId, projectRequest.SortOrder);
            }
        }
    }

    private static AiScoreStaleness GetBulletStaleness(ResumeCompanyBullet existing, BulletSelectionRequest request)
    {
        var wasAlternativeSwap =
            existing.Value == request.AlternativeValue &&
            existing.AlternativeValue == request.Value;

        if (!wasAlternativeSwap &&
            (existing.Value != request.Value ||
             existing.AlternativeValue != request.AlternativeValue ||
             existing.SourceBulletId != request.BulletId))
        {
            return AiScoreStaleness.High;
        }

        if (existing.SortOrder != request.SortOrder)
        {
            return AiScoreStaleness.Medium;
        }

        if (wasAlternativeSwap)
        {
            return AiScoreStaleness.Low;
        }

        return AiScoreStaleness.None;
    }
}
