
using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Profile.Education.Interfaces;
using EducationEntity = ResumeTailor.Domain.Profile.LibraryEducation;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

public class EducationRepository(ResumeTailorDbContext dbContext) : IEducationRepository
{
    public async Task<IReadOnlyCollection<LibraryEducationResponse>> GetListByAccountIdAsync(int accountId, bool archived, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .AsNoTracking()
            .Where(e => e.AccountId == accountId && (archived == (e.ArchiveDate != null)))
            .Select(e => new LibraryEducationResponse(
                Id: e.Id,
                Type: e.Type,
                InstitutionName: e.InstitutionName,
                Qualification: e.Qualification,
                FieldOfStudy: e.FieldOfStudy,
                Gpa: e.Gpa,
                Location: e.Location,
                Started: e.Started,
                Ended: e.Ended,
                UseForResume: e.UseForResume,
                ResumeCount: dbContext.Resumes.Count(
                    r => r.AccountId == accountId && (r.EducationSelections.Any(
                        es => es.LibraryEducationId == e.Id)
                    ))
                ))
            .ToListAsync(cancellationToken);
    }

    public async Task<EducationEntity?> GetForUpdatingByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<int>> GetIdsForCreationAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .AsNoTracking()
            .Where(e => e.AccountId == accountId && e.UseForResume)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUsedInResumeAsync(int educationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes.AnyAsync(r => r.EducationSelections.Any(es => es.LibraryEducationId == educationId));
    }

    public void Add(EducationEntity education)
    {
        dbContext.Education.Add(education);
    }

    public void Remove(EducationEntity education)
    {
        dbContext.Education.Remove(education);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
