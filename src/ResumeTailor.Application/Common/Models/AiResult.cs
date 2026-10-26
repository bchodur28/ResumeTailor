namespace ResumeTailor.Application.Common.Models;

public record AiResult<T>(AiMetaData MetaData, T Value);
