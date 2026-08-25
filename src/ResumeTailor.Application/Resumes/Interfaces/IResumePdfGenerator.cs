namespace ResumeTailor.Application.Resumes.Interfaces;

public interface IResumePdfGenerator
{
    byte[] Generate();
    void SaveAsFile();
}
