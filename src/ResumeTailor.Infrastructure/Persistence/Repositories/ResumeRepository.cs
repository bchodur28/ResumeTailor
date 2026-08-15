using ResumeTailor.Application.Pdf.Interfaces;
using ResumeTailor.Domain.Resume;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Repositories;

public class ResumeRepository(ResumeTailorDbContext dbContext) : IResumeRepository
{
    public Task<Resume?> GetResumeById(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateCompany(Company company, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCompany(Company company, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CreateBullet(Bullet bullet, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBullet(Bullet bullet, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    
}
