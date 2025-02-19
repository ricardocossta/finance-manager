using FinanceManger.Application.Common.Interfaces;
using FinanceManger.Domain.Transactions;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Queries.GetTransaction;

public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, Result<Transaction>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<Transaction>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);

        return transaction is null
            ? Result.Fail(new Error($"Transaction with id {request.Id} not found.")
                .WithMetadata("Error", TransactionErrors.TransactionNotFound))
            : transaction;
    }
}
