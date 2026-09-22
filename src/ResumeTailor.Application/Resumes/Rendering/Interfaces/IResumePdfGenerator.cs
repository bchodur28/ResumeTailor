namespace ResumeTailor.Application.GeneratedResumes.Rendering.Interfaces;

public interface IResumePdfGenerator
{
    byte[] Generate();
    void SaveAsFile();
}
