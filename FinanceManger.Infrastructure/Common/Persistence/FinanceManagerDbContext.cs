using FinanceManger.Application.Common.Interfaces;
using FinanceManger.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinanceManger.Infrastructure.Common.Persistence;

public class FinanceManagerDbContext : DbContext, IUnitOfWork
{
    public DbSet<Transaction> Transactions { get; set; }

    public FinanceManagerDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
