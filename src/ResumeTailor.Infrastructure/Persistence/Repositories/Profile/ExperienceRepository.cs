using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Profile.Experience.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

internal class ExperienceRepository(ResumeTailorDbContext dbContext) : IExperienceRepository
{
    // Company
    public async Task<IReadOnlyCollection<Company>> GetCompaniesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Company>> GetCompaniesWithNonDeletedBulletsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .Include(c => c.Bullets.Where(b => !b.IsDeleted))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CompanyWithBulletCount>> GetCompaniesWithCountByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .Select(c => new CompanyWithBulletCount(c, c.Bullets.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Company>> GetCompaniesForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .Where(c => c.AccountId == accountId)
            .Include(c => c.Bullets)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Company>> GetCompaniesWithBulletsIncludingDeletedByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .Include(c => c.Bullets)
            .ToListAsync(cancellationToken);
    }

    public void AddCompanies(IEnumerable<Company> companies)
    {
        dbContext.Companies.AddRange(companies);
    }

    public void RemoveCompanies(IEnumerable<Company> companies)
    {
        dbContext.Companies.RemoveRange(companies);
    }

    // Project
    public async Task<IReadOnlyCollection<Project>> GetProjectsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .Where(p => p.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Project>> GetProjectsForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .Where(p => p.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.Projects
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public void AddProjects(IEnumerable<Project> projects)
    {
        dbContext.Projects.AddRange(projects);
    }

    public void RemoveProjects(IEnumerable<Project> projects)
    {
        dbContext.Projects.RemoveRange(projects);
    }

    // Bullet
    public async Task<IReadOnlyCollection<CompanyBullets>> GetCompanyBulletsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var companies = await dbContext.Companies
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .Select(c => new CompanyBullets(
                CompanyId: c.Id,
                CompanyName: c.Name,
                Bullets: c.Bullets
                    .Select(b => new BulletWithUsage(
                        Id: b.Id,
                        CompanyId: b.CompanyId,
                        Value: b.Value,
                        AiScore: b.AiScore,
                        IsDeleted: b.IsDeleted,
                        ResumeCount: dbContext.ResumeBullets
                            .Where(rb => rb.SourceBulletId == b.Id)
                            .Join(
                                dbContext.ResumeCompanySelections,
                                rb => rb.CompanyId,
                                rcs => rcs.Id,
                                (rb, rcs) => rcs.ResumeId
                            )
                            .Distinct()
                            .Count()
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return companies;
    }

    public async Task<IReadOnlyCollection<Bullet>> GetBulletsForUpdatingByCompanyIdsAsync(int accountId, HashSet<int> companyIds, HashSet<int> bulletIds, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bullets
        .Where(b =>
            b.DeletedDate == null &&
            bulletIds.Contains(b.Id) &&
            companyIds.Contains(b.CompanyId) &&
            dbContext.Companies.Any(c =>
                c.Id == b.CompanyId &&
                c.AccountId == accountId))
        .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Bullet>> GetBulletsNotReferencedByResumeAsync(HashSet<int> bulletIds, CancellationToken cancellationToken = default)
    {
        return await dbContext.Bullets
            .Where(bullet =>
                bulletIds.Contains(bullet.Id) &&
                !dbContext.ResumeBullets.Any(resumeBullet => resumeBullet.SourceBulletId == bullet.Id))
            .ToListAsync(cancellationToken);
    }

    public void AddBullets(IEnumerable<Bullet> bullets)
    {
        dbContext.Bullets.AddRange(bullets);
    }

    public void RemoveBullets(IEnumerable<Bullet> bullets)
    {
        dbContext.Bullets.RemoveRange(bullets);
    }

    //Save changes
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
