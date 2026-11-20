namespace ResumeTailor.Application.Contracts.Accounts;

public sealed record PersonalLinkRequest(int? Id, string DisplayName, string Url);
