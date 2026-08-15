using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Pdf.Interfaces;
using ResumeTailor.Domain.Resume;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Repositories;

public class ResumeRepository(ResumeTailorDbContext dbContext) : IResumeRepository
{
    public async Task<Resume?> GetResumeById(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .Include(r => r.Company)
                .ThenInclude(c => c.Bullets)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task CreateCompany(Company company, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(company);
    }

    public async Task CreateBullet(Bullet bullet, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(bullet);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
