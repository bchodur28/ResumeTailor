namespace ResumeTailor.Application.Contracts.Accounts;

public record CreditWalletResponse(int Id, int AccountId, int Balance, int ReservedCredits, int AvailableCredits);
