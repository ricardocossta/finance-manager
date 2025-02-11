using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid TransactionId) : IRequest<Result>;
