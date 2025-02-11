namespace FinanceManger.Contracts.Transactions;

public record CreateTransactionRequest(
    Guid UserId,
    string Description,
    decimal Amount,
    TransactionType Type
);