using FluentResults;

namespace FinanceManger.Domain.Transactions;

public class Transaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    private Transaction() : base() { }

    private Transaction(string description, decimal amount, TransactionType type, Guid userId, Guid? id = null) : base(id)
    {
        Description = description;
        Amount = amount;
        Type = type;
        UserId = userId;
    }

    public static Result<Transaction> Create(string description, decimal amount, TransactionType type, Guid userId, Guid? id = null)
    {
        if (amount <= 0)
        {
            return Result.Fail(TransactionErrors.AmountMustBeGreaterThanZero);
        }

        var transaction = new Transaction(description, amount, type, userId, id);
        return Result.Ok(transaction);
    }

    public Result Update(string description, decimal amount, TransactionType type)
    {
        if (amount <= 0)
        {
            return Result.Fail(TransactionErrors.AmountMustBeGreaterThanZero);
        }

        Description = description;
        Amount = amount;
        Type = type;
        UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}
