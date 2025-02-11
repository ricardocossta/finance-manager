namespace FinanceManger.Contracts.Transactions;

public record TransactionResponse(
    Guid Id,
    Guid UserId,
    string Description,
    decimal Amount,
    TransactionType Type,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);