using FinanceManger.Application.Common.Interfaces;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);

        if (transaction is null)
        {
            return Result.Fail($"Transaction with id {request.Id} not found.");
        }

        transaction.Update(request.Description, request.Amount, request.Type);

        _transactionRepository.Update(transaction);
        await _unitOfWork.CommitChangesAsync();

        return Result.Ok();
    }
}
