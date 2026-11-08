

namespace ResumeTailor.Domain.Common.Exceptions;

public class ExperienceInUseException : Exception
{
    public ExperienceInUseException() : base("This experience is used in existing resumes. Archive it instead.") { }
}
