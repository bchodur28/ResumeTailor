using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Profile;

internal class LibraryExperienceRepository(ResumeTailorDbContext dbContext) : ILibraryExperienceRepository
{
    public async Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetSummeriesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.LibraryExperiences
            .AsNoTracking()
            .Where(ex => ex.AccountId == accountId)
            .Select(ex => new LibraryExperienceSummaryResponse(
                Id: ex.Id,
                Type: ex.Type,
                Title: ex.Title,
                Organaization: ex.Organization,
                SummaryCount: ex.Summaries.Count(s => s.DeletedDate == null),
                LetAiChooseSummary: ex.LetAiChooseSummary,
                BulletCount: ex.Bullets.Count(s => s.DeletedDate == null),
                LetAiChooseBullets: ex.LetAiChooseBullets,
                MaxBulletsToChoose: ex.MaxBulletsToChoose))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<LibraryExperience>> GetAllByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.LibraryExperiences
            .AsSplitQuery()
            .AsNoTracking()
            .Where(ex => ex.AccountId == accountId)
            .Include(ex => ex.Summaries)
            .Include(ex => ex.Bullets)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<LibraryExperienceListItemResponse>> GetListItemsAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.LibraryExperiences
            .AsNoTracking()
            .Where(ex => ex.AccountId == accountId)
            .Select(ex => new LibraryExperienceListItemResponse(
                ex.Id,
                ex.Type,
                ex.Title,
                ex.Organization,
                ex.Bullets.Count(b => b.DeletedDate == null),
                ex.Summaries.Count(b => b.DeletedDate == null)))
            .ToListAsync();
    }

    public async Task<LibraryExperience?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.LibraryExperiences
            .AsSplitQuery()
            .AsNoTracking()
            .Where(ex => ex.Id == id)
            .Include(ex => ex.Summaries)
            .Include(ex => ex.Bullets)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<LibraryExperience>> GetByIdsAsync(IReadOnlyCollection<int> Ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.LibraryExperiences
            .AsSplitQuery()
            .AsNoTracking()
            .Where(ex => Ids.Contains(ex.Id))
            .Include(ex => ex.Summaries)
            .Include(ex => ex.Bullets)
            .ToListAsync(cancellationToken);
    }



    public async Task<IReadOnlyCollection<LibraryExperienceAiInput>>GetAiInputDataForGeneration(int accountId, CancellationToken cancellationToken = default)
    {
        var experiences = await dbContext.LibraryExperiences
            .AsNoTracking()
            .Where(lex => lex.AccountId == accountId)
            .Include(lex => lex.Summaries
                .Where(sum => sum.DeletedDate == null))
            .Include(lex => lex.Bullets
                .Where(b => b.DeletedDate == null))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return experiences
            .Select(lex => new LibraryExperienceAiInput(
                ExperienceId: lex.Id,

                LetAiChooseSummary: lex.LetAiChooseSummary,

                Summaries: lex.Summaries
                    .Where(sum =>
                        lex.LetAiChooseSummary || sum.IsPrimary)
                    .ToDictionary(sum => sum.Id, sum => sum.Value),

                LetAiChooseBullets: lex.LetAiChooseBullets,

                MaxBullets: lex.LetAiChooseBullets
                    ? lex.MaxBulletsToChoose
                    : null,

                Bullets: lex.Bullets
                    .ToDictionary(b => b.Id, b => b.Value),

                Started: lex.Started,
                Ended: lex.Ended
            ))
            .ToList();
    }
    
    public async Task<bool> AreSelectionsValidAsync(int accountId, IReadOnlyCollection<ExperienceSelectionValidation> selections, CancellationToken cancellationToken = default)
    {
        var experienceIds = selections
            .Select(s => s.LibraryExperienceId)
            .Distinct()
            .ToArray();

        var summaryIds = selections
            .Where(s => s.LibrarySummaryId.HasValue)
            .Select(s => s.LibrarySummaryId!.Value)
            .Distinct()
            .ToArray();

        var bulletIds = selections
            .SelectMany(s => s.LibraryBulletIds)
            .Distinct()
            .ToArray();

        // Validate experience ownership.
        var validExperienceIds = await dbContext.LibraryExperiences
            .AsNoTracking()
            .Where(e =>
                e.AccountId == accountId &&
                experienceIds.Contains(e.Id))
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (validExperienceIds.Count != experienceIds.Length)
            return false;

        // Retrieve summary ownership.
        var summaries = await dbContext.LibrarySummaries
            .AsNoTracking()
            .Where(s =>
                summaryIds.Contains(s.Id) &&
                s.DeletedDate == null)
            .Select(s => new
            {
                s.Id,
                s.ExperienceId
            })
            .ToListAsync(cancellationToken);

        // Retrieve bullet ownership.
        var bullets = await dbContext.LibraryBullets
            .AsNoTracking()
            .Where(b =>
                bulletIds.Contains(b.Id) &&
                b.DeletedDate == null)
            .Select(b => new
            {
                b.Id,
                b.ExperienceId
            })
            .ToListAsync(cancellationToken);

        var summaryLookup = summaries.ToDictionary(s => s.Id, s => s.ExperienceId);
        var bulletLookup = bullets.ToDictionary(b => b.Id, b => b.ExperienceId);

        return selections.All(selection =>
            (!selection.LibrarySummaryId.HasValue ||
             (summaryLookup.TryGetValue(
                 selection.LibrarySummaryId.Value,
                 out var summaryExperienceId
             ) && summaryExperienceId == selection.LibraryExperienceId))
            &&
            selection.LibraryBulletIds.All(bulletId =>
                bulletLookup.TryGetValue(bulletId, out var bulletExperienceId)
                && bulletExperienceId == selection.LibraryExperienceId));
    }



    public async Task<LibraryExperience?> GetForUpdatingByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.LibraryExperiences
            .AsSplitQuery()
            .Where(ex => ex.Id == id)
            .Include(ex => ex.Summaries)
            .Include(ex => ex.Bullets)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void Add(LibraryExperience experiences)
    {
        dbContext.LibraryExperiences.Add(experiences);
    }

    public void Remove(LibraryExperience experience)
    {
        dbContext.LibraryExperiences.Remove(experience);
    }

    public async Task SaveAync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
