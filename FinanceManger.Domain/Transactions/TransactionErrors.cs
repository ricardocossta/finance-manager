using FluentResults;

namespace FinanceManger.Domain.Transactions;

public static class TransactionErrors
{
    public static readonly Error AmountMustBeGreaterThanZero = new Error(
        "Amount must be greater than 0")
        .WithMetadata("ErrorCode", nameof(AmountMustBeGreaterThanZero));
}
