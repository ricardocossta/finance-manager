using FinanceManger.Application.Common.Interfaces;
using FinanceManger.Domain.Transactions;
using FinanceManger.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManger.Infrastructure.Transactions.Persistence;

public class TransactionRepository : ITransactionRepository
{
    private readonly FinanceManagerDbContext _dbContext;

    public TransactionRepository(FinanceManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        return await _dbContext.Transactions.FindAsync(transactionId);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _dbContext.Transactions.ToListAsync();
    }

    public void Delete(Transaction transaction)
    {
        _dbContext.Transactions.Remove(transaction);
    }

    public void Update(Transaction transaction)
    {
        _dbContext.Transactions.Update(transaction);
    }
}
