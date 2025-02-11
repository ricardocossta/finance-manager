using FinanceManger.Domain.Transactions;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(Guid Id) : IRequest<Result<Transaction>>;