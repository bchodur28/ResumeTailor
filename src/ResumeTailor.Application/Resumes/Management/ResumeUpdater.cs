using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.Contracts.Resume.Selection;
using ResumeTailor.Application.Resumes.Management.Interfaces;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes.Management;

public sealed class ResumeUpdater : IResumeUpdater
{
    public void Update(Resume resume, UpdateResumeRequest request)
    {
        resume.Update(request.Name);
        UpdateAppearance(request, resume);

        UpdateEducationSelections(resume, request.Education);
        UpdateExperience(resume, request.Experiences);
    }

    private static void UpdateAppearance(UpdateResumeRequest request, Resume existingResume)
    {
        existingResume.Appearance?.Update(
                    titleFontSize: request.Appearance.TitleFontSize,
                    sectionHeaderFontSize: request.Appearance.SectionHeaderFontSize,
                    mainBodyFontSize: request.Appearance.MainBodyFontSize,
                    fontFamily: request.Appearance.FontFamily,
                    fontColor: request.Appearance.FontColor,
                    topHeaderAlignment: request.Appearance.TopHeaderAlignment,
                    hideCertificationIssuer: request.Appearance.HideCertificationIssuer);
    }


    private static void UpdateExperience(Resume resume, IReadOnlyCollection<ResumeExperienceRequest> experienceRequests)
    {
        var requestedExperienceIds = experienceRequests
            .Where(r => r.Id.HasValue)
            .Select(r => r.Id!.Value)
            .ToHashSet();

        var experiencesToDelete = resume.Experiences
            .Where(s => !requestedExperienceIds.Contains(s.Id))
            .ToList();

        foreach (var experience in experiencesToDelete)
        {
            resume.MarkAiScoreStale(AiScoreStaleness.High);
            resume.RemoveExperience(experience);
        }

        foreach (var experienceRequest in experienceRequests)
        {
            if (experienceRequest.Id is int experienceId)
            {
                var existingExperience = resume.Experiences
                    .FirstOrDefault(e => e.Id == experienceId)
                    ?? throw new NotFoundException(
                        $"Resume experience {experienceId} was not found.");

                if (existingExperience.LibraryExperienceId != experienceRequest.LibraryExperienceId)
                {
                    throw new InvalidOperationException(
                        "An existing experience cannot change its LibraryExperienceId.");
                }

                // Summary changes
                if (existingExperience.ResumeSummary?.LibrarySummaryId != experienceRequest.LibrarySummaryId)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                    existingExperience.UpdateSummary(experienceRequest.LibrarySummaryId);
                }

                // Experience ordering
                if (existingExperience.SortOrder != experienceRequest.SortOrder)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.Medium);
                }

                existingExperience.Update(experienceRequest.SortOrder);

                // Existing bullet processing goes here.

                var requestedBulletIds = experienceRequest.Bullets
                    .Where(b => b.Id.HasValue)
                    .Select(b => b.Id!.Value)
                    .ToHashSet();

                var bulletsToDelete = existingExperience.Bullets
                    .Where(b => !requestedBulletIds.Contains(b.Id))
                    .ToList();

                foreach (var bullet in bulletsToDelete)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                    existingExperience.RemoveBullet(bullet);
                }

                foreach (var bulletRequest in experienceRequest.Bullets)
                {
                    if (bulletRequest.Id is int bulletId)
                    {
                        var existingBullet = existingExperience.Bullets
                            .FirstOrDefault(b => b.Id == bulletId)
                            ?? throw new NotFoundException(
                                $"Resume bullet {bulletId} was not found.");

                        if (existingBullet.LibraryBulletId != bulletRequest.LibraryBulletId)
                        {
                            resume.MarkAiScoreStale(AiScoreStaleness.High);
                        } else if (existingBullet.SortOrder != bulletRequest.SortOrder)
                        {
                            resume.MarkAiScoreStale(AiScoreStaleness.Medium);
                        }

                        existingBullet.Update(
                            bulletRequest.LibraryBulletId,
                            bulletRequest.SortOrder
                        );
                    } else
                    {
                        resume.MarkAiScoreStale(AiScoreStaleness.High);

                        existingExperience.AddBullet(
                            libraryBulletId: bulletRequest.LibraryBulletId,
                            alternative: null,
                            reasoning: null,
                            sortOrder: bulletRequest.SortOrder
                        );
                    }
                }

            } else
            {
                resume.MarkAiScoreStale(AiScoreStaleness.High);

                var newExperience = resume.AddExperience(
                    experienceRequest.LibraryExperienceId,
                    experienceRequest.LibrarySummaryId,
                    experienceRequest.SortOrder
                );

                foreach (var bulletRequest in experienceRequest.Bullets)
                {
                    newExperience.AddBullet(
                        libraryBulletId: bulletRequest.LibraryBulletId,
                        alternative: null,
                        reasoning: null,
                        sortOrder: bulletRequest.SortOrder
                    );
                }
            }
        }
    }

    private static void UpdateEducationSelections(Resume resume, IReadOnlyCollection<ResumeEducationRequest> educationSelectionRequests)
    {
        var requestedEducationIds = educationSelectionRequests
            .Select(r => r.LibraryEducationId)
            .ToHashSet();

        var educationSelectionsToDelete = resume.EducationSelections
            .Where(s => !requestedEducationIds.Contains(s.LibraryEducationId))
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
                    ?? throw new NotFoundException($"Education selection with ID {educationRequest.LibraryEducationId} was not found while updating.");

                existingEducationSelection.Update(educationRequest.LibraryEducationId, educationRequest.SortOrder);
            } else
            {
                resume.AddEducationSelection(educationRequest.LibraryEducationId, educationRequest.SortOrder);
            }
        }
    }
}
