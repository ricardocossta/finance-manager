using FinanceManger.Domain.Transactions;
using MediatR;

namespace FinanceManger.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery() : IRequest<List<Transaction>>;