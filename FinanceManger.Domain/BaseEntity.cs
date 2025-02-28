namespace FinanceManger.Domain;
public class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected BaseEntity() { }

    public BaseEntity(Guid? id)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}
