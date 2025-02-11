using FinanceManger.Domain.Transactions;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid Id,
    string Description,
    decimal Amount,
    TransactionType Type) : IRequest<Result>;