namespace FinanceManger.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}
