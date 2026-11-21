namespace ResumeTailor.Domain.Common.Exceptions
{
    public sealed class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(string message) : base(message) { }
    }
}
