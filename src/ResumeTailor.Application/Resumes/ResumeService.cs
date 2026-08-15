using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes;

internal sealed class ResumeService(IResumeRepository repository) : IResumeService
{
    public async Task<ResumeResponse> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await repository.GetResumeByIdAsync(id);

        if(response is null)
        {
            throw new NotFoundException($"Resume with given ID {id} was not found");
        }

        return MapToResumeResponse(response);
    }

    public async Task CreateResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default)
    {
        var resume = new Resume(
            request.PersonName,
            request.Profession,
            request.Email,
            request.PhoneNumber,
            request.College,
            request.Degree,
            request.Major,
            request.CollegeStatus,
            request.PersonalSite1,
            request.PersonalSite2,
            request.PersonalSite3
        );
        await repository.CreateResume(resume, cancellationToken);
    }

    public async Task CreateCompanyAsync(CompanyRequest request, CancellationToken cancellationToken = default)
    {
        var resumeExists = await repository.ResumeExistsAsync(request.ResumeId);

        if (!resumeExists)
        {
            throw new NotFoundException($"Resume with given ID {request.ResumeId} was not found during creation of Company");
        }

        var company = new Company(
            request.ResumeId,
            request.Name,
            request.Position,
            request.WorkingStatus,
            request.Location,
            request.GenerateBullets,
            request.MaxGeneratedBulletCount
        );

        await repository.CreateCompanyAsync(company, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task CreateBulletAsync(BulletRequest request, CancellationToken cancellationToken = default)
    {
        var companyExists = await repository.CompanyExistsAsync(request.CompanyId);
        if (!companyExists)
        {
            throw new NotFoundException($"Company with given ID {request.CompanyId} was not found during creation of Bullet");
        }
        var bullet = new Bullet(
            request.CompanyId,
            request.Value
        );
        await repository.CreateBulletAsync(bullet, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    private static ResumeResponse MapToResumeResponse(Resume resume)
    {
        return new ResumeResponse(
            resume.Id,
            resume.PersonName,
            resume.Profession,
            resume.Email,
            resume.PhoneNumber,
            resume.College,
            resume.Degree,
            resume.Major,
            resume.CollegeStatus,
            resume.PersonalSite1,
            resume.PersonalSite2,
            resume.PersonalSite3,
            Skills: [],
            resume.Companies.Select(c => new CompanyResponse(
                c.Id,
                c.Name,
                c.Position,
                c.WorkingStatus,
                c.Location,
                c.GenerateBullets,
                c.MaxGeneratedBulletCount,
                c.Bullets.Select(b => new BulletResponse(
                    b.Id,
                    b.Value
                    )).ToList()
            )).ToList()
        );
    }
}
