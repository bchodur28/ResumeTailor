

using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Mappers;

public static class LibraryExperienceMapper
{
    public static LibraryExperience MapRequestToEntity(int accountId, LibraryExperienceRequest request)
    {
        return new LibraryExperience(
            accountId: accountId,
            type: request.Type,
            title: request.Title,
            organization: request.Organization,
            location: request.Location,
            started: request.Started,
            ended: request.Ended,
            letAiChooseSummary: request.LetAiChooseSummary,
            link: request.Link,
            techStack: request.TechStack,
            letAiChooseBullets: request.LetAiChooseBullets,
            maxBulletsToChoose: request.MaxBulletsToChoose
            );
    }

    public static LibrarySummary MapSummaryRequestToEntity(LibrarySummaryRequest request)
    {
        return new LibrarySummary(request.ExperienceId, request.Value);
    }

    public static LibraryBullet MapBulletRequestToEntity(LibraryBulletRequest request)
    {
        return new LibraryBullet(request.ExperienceId, request.Value);
    }

    public static LibraryExperienceResponse MapEntityToResponse(LibraryExperience entity)
    {
        var primarySummary = entity.PrimarySummary;
        return new LibraryExperienceResponse(
            Id: entity.Id,
            Type: entity.Type,
            Title: entity.Title,
            Organization: entity.Organization,
            Location: entity.Location,
            Started: entity.Started,
            Ended: entity.Ended,
            PrimarySummary: primarySummary is not null
                ? new LibrarySummaryResponse(
                    Id: primarySummary.Id,
                    IsPrimary: primarySummary.IsPrimary,
                    Value: primarySummary.Value,
                    AiRewrite: primarySummary.AiRewrite,
                    AiOpinion: primarySummary.AiOpinion,
                    AiScore: primarySummary.AiScore,
                    IsDeleted: primarySummary.IsDeleted)
                : null,
            LetAiChooseSummary: entity.LetAiChooseSummary,
            Summeries: entity.Summaries.Select(sum => new LibrarySummaryResponse(
                    Id: sum.Id,
                    IsPrimary: sum.IsPrimary,
                    Value: sum.Value,
                    AiRewrite: sum.AiRewrite,
                    AiOpinion: sum.AiOpinion,
                    AiScore: sum.AiScore,
                    IsDeleted: sum.IsDeleted)
                ).ToList(),
            Link: entity.Link,
            TechStack: entity.TechStack,
            LetAiChooseBullets: entity.LetAiChooseBullets,
            MaxBulletsToChoose: entity.MaxBulletsToChoose,
            Bullets: entity.Bullets.Select(b => new LibraryBulletResponse(
                Id: b.Id,
                Value: b.Value,
                AiRewrite: b.AiRewrite,
                AiOpinion: b.AiOpinion,
                AiScore: b.AiScore,
                IsDeleted: b.IsDeleted)
            ).ToList()
        );
    }
}
