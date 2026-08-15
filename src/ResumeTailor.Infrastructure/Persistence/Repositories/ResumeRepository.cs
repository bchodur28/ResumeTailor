using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Domain.Resumes;


namespace ResumeTailor.Infrastructure.Persistence.Repositories;

public class ResumeRepository(ResumeTailorDbContext dbContext) : IResumeRepository
{
    public async Task<Resume?> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsNoTracking()
            .Include(r => r.Companies)
                .ThenInclude(c => c.Bullets)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task CreateResume(Resume resume, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(resume);
    }

    public async Task<bool> ResumeExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task CreateCompanyAsync(Company company, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(company);
    }

    public async Task<bool> CompanyExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companys.AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task CreateBulletAsync(Bullet bullet, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(bullet);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
