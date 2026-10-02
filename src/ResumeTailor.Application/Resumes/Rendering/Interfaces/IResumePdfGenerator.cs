using ResumeTailor.Application.Contracts.Resume;

namespace ResumeTailor.Application.GeneratedResumes.Rendering.Interfaces;

public interface IResumePdfGenerator
{
    byte[] Generate(ResumeResponse resume);
}
