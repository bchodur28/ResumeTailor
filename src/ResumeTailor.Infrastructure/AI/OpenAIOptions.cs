

namespace ResumeTailor.Infrastructure.AI;

public sealed class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public required string Model { get; init; }
}
