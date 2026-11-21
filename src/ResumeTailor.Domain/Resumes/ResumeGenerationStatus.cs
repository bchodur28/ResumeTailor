namespace ResumeTailor.Domain.Resumes;

public enum ResumeGenerationStatus
{
    Generating = 1,
    ReadyToFinalize = 2,
    Completed = 3,
    Failed = 4
}
