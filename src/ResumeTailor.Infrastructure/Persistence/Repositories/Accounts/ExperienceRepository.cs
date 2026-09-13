using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

internal class ExperienceRepository(ResumeTailorDbContext dbContext) : IExperienceRepository
{
    // Company
    public async Task<IReadOnlyCollection<Company>> GetCompaniesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountCompanies
            .AsNoTracking()
            .Where(c => c.AccountId == accountId)
            .Include(c => c.Bullets)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Company>> GetCompaniesByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountCompanies
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .Include(c => c.Bullets)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateCompanyAsync(Company company, CancellationToken cancellationToken = default)
    {
        await dbContext.AccountCompanies.AddAsync(company, cancellationToken);
    }

    public async Task DeleteCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        var company = await dbContext.AccountCompanies.FindAsync([id], cancellationToken);
        if (company is null)
        {
            return;
        }
        dbContext.AccountCompanies.Remove(company);
    }

    public async Task<bool> CompanyExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountCompanies.AnyAsync(c => c.Id == id, cancellationToken);
    }

    // Bullet
    public async Task CreateBulletAsync(Bullet bullet, CancellationToken cancellationToken = default)
    {
        await dbContext.AccountBullets.AddAsync(bullet, cancellationToken);
    }

    public async Task DeleteBulletAsync(int id, CancellationToken cancellationToken = default)
    {
        var bullet = await dbContext.AccountBullets.FindAsync([id], cancellationToken);
        if (bullet is null)
        {
            return;
        }
        dbContext.AccountBullets.Remove(bullet);
    }

    public async Task<bool> BulletExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountBullets.AnyAsync(b => b.Id == id, cancellationToken);
    }

    // Education
    public async Task<IReadOnlyCollection<Education>> GetEducationByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountEducations
            .AsNoTracking()
            .Where(e => e.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Education>> GetEducationByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountEducations
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }

    // Project
    public async Task<IReadOnlyCollection<Project>> GetProjectsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountProjects
            .AsNoTracking()
            .Where(p => p.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountProjects
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task CreateProjectAsync(Project project, CancellationToken cancellationToken = default)
    {
        await dbContext.AccountProjects.AddAsync(project, cancellationToken).AsTask();
    }

    public async Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default)
    {
        var project = await dbContext.AccountProjects.FindAsync([id], cancellationToken);
        if (project is null)
        {
            return;
        }
        dbContext.AccountProjects.Remove(project);
    }

    public async Task<bool> ProjectExistAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountProjects.AnyAsync(p => p.Id == id, cancellationToken);
    }

    //Save changes
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
