﻿using FinanceManger.Application.Common.Interfaces;
using FinanceManger.Domain.Transactions;
using FluentResults;
using MediatR;

namespace FinanceManger.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<Transaction>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Transaction>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var createTransactionResult = Transaction.Create(
            request.Description,
            request.Amount,
            request.Type,
            request.UserId
        );

        if (createTransactionResult.IsFailed)
        {
            return createTransactionResult;
        }

        await _transactionRepository.AddAsync(createTransactionResult.Value);
        await _unitOfWork.CommitChangesAsync();

        return createTransactionResult;
    }
}
