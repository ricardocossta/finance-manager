using FinanceManger.Application.Common.Interfaces;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);

        if (transaction is null)
        {
            return Result.Fail($"Transaction with id {request.TransactionId} not found.");
        }

        _transactionRepository.Delete(transaction);
        await _unitOfWork.CommitChangesAsync();

        return Result.Ok();
    }
}
