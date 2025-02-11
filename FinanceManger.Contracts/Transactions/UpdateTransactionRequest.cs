namespace FinanceManger.Contracts.Transactions;
public record UpdateTransactionRequest(
    string Description,
    decimal Amount,
    TransactionType Type
);
