namespace ResumeTailor.Application.Resumes.Exceptions;

public sealed class AiNotConfiguredException : Exception
{
    public AiNotConfiguredException() : base("AI functionality has not been configured.")
    {

    }

    public AiNotConfiguredException(string message) : base(message) { }
}
