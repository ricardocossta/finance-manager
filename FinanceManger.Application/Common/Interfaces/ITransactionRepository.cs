using FinanceManger.Domain.Transactions;

namespace FinanceManger.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);

    Task<Transaction?> GetByIdAsync(Guid transactionId);

    Task<List<Transaction>> GetAllAsync();

    void Delete(Transaction transaction);

    void Update(Transaction transaction);
}
