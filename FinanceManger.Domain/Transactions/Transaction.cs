namespace FinanceManger.Domain.Transactions;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    private Transaction() { }

    public Transaction(string description, decimal amount, TransactionType type, Guid userId, Guid? id = null)
    {
        Description = description;
        Amount = amount;
        Type = type;
        UserId = userId;
        Id = id ?? Guid.NewGuid();
    }

    public void Update(string description, decimal amount, TransactionType type)
    {
        Description = description;
        Amount = amount;
        Type = type;
    }
}
