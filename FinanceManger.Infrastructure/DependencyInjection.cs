using FinanceManger.Application.Common.Interfaces;
using FinanceManger.Infrastructure.Common.Persistence;
using FinanceManger.Infrastructure.Transactions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<FinanceManagerDbContext>(options =>
        {
            options.UseSqlite("Data Source = FinanceManager.db");
        });

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<FinanceManagerDbContext>());

        return services;
    }
}
