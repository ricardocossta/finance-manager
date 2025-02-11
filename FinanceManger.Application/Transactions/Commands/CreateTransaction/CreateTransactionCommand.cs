using FinanceManger.Domain.Transactions;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    string Description, decimal Amount, TransactionType Type, Guid UserId) : IRequest<Result<Transaction>>;