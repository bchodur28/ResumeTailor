using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.Resumes.Management.Interfaces;

public interface IResumeUpdater
{
    void Update(Resume resume, UpdateResumeRequest request);
}
