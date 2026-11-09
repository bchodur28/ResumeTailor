namespace ResumeTailor.Domain.Common.Exceptions;

public class EducationInUseException : Exception
{
    public EducationInUseException() : base("This experience is used in existing resumes. Archive it instead.") { }
}
