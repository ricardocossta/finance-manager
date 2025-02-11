namespace FinanceManger.Domain.Transactions;

public class Transaction : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    private Transaction() : base() { }

    public Transaction(string description, decimal amount, TransactionType type, Guid userId, Guid? id = null) : base(id)
    {
        Description = description;
        Amount = amount;
        Type = type;
        UserId = userId;
    }

    public void Update(string description, decimal amount, TransactionType type)
    {
        Description = description;
        Amount = amount;
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }
}
