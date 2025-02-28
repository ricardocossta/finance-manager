using FinanceManger.Domain.Shared;

namespace FinanceManger.Domain.Transactions;

public static class TransactionErrors
{
    public static readonly AppError AmountMustBeGreaterThanZero = new(
        "Amount must be greater than 0", ErrorType.Validation);

    public static readonly AppError TransactionNotFound = new(
        "Transaction not found", ErrorType.NotFound);
}
