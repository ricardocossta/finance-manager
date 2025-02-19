using FinanceManger.Application.Transactions.Commands.CreateTransaction;
using FinanceManger.Application.Transactions.Commands.DeleteTransaction;
using FinanceManger.Application.Transactions.Commands.UpdateTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransactions;
using FinanceManger.Contracts.Transactions;
using FinanceManger.Domain.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManger.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ISender _mediator;

    public TransactionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        if (!Enum.IsDefined(typeof(Domain.Transactions.TransactionType), (int)request.Type))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid transaction type");
        }

        var transactionType = Enum.Parse<Domain.Transactions.TransactionType>(request.Type.ToString());

        var command = new CreateTransactionCommand(
            request.Description, request.Amount, transactionType, request.UserId);

        var createTransactionResult = await _mediator.Send(command);

        return createTransactionResult.IsFailed
            ? Problem()
            : CreatedAtAction(
                nameof(GetTransaction),
                new { transactionId = createTransactionResult.Value.Id },
                MapTransactionToTransactionResponse(createTransactionResult.Value));
    }

    [HttpGet("{transactionId}")]
    public async Task<IActionResult> GetTransaction(Guid transactionId)
    {
        var query = new GetTransactionQuery(transactionId);

        var getTransactionResult = await _mediator.Send(query);

        if (getTransactionResult.IsFailed)
        {
            var error = getTransactionResult.Errors[0];
            
            if (error.Metadata.TryGetValue("Error", out var errorType) && errorType is TransactionErrors.TransactionNotFound)
            {
                return NotFound(error.Message);
            }
        }

        return Ok(MapTransactionToTransactionResponse(getTransactionResult.Value));
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var query = new GetTransactionsQuery();

        var getTransactionsResult = await _mediator.Send(query);

        if (getTransactionsResult.Count == 0)
        {
            return NoContent();
        }

        return Ok(getTransactionsResult.Select(MapTransactionToTransactionResponse));
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        var command = new DeleteTransactionCommand(transactionId);
        var deleteTransactionResult = await _mediator.Send(command);

        if (deleteTransactionResult.IsFailed)
        {
            var error = deleteTransactionResult.Errors[0];

            if (error.Metadata.TryGetValue("Error", out var errorType) && errorType is TransactionErrors.TransactionNotFound)
            {
                return NotFound(error.Message);
            }
        }

        return NoContent();
    }

    [HttpPut("{transactionId}")]
    public async Task<IActionResult> UpdateTransaction(Guid transactionId, [FromBody] UpdateTransactionRequest request)
    {
        if (!Enum.IsDefined(typeof(Domain.Transactions.TransactionType), (int)request.Type))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid transaction type");
        }

        var transactionType = Enum.Parse<Domain.Transactions.TransactionType>(request.Type.ToString());

        var command = new UpdateTransactionCommand(
            transactionId, request.Description, request.Amount, transactionType);

        var updateTransactionResult = await _mediator.Send(command);

        if (updateTransactionResult.IsFailed)
        {
            var error = updateTransactionResult.Errors[0];

            if (error.Metadata.TryGetValue("Error", out var errorType) && errorType is TransactionErrors.TransactionNotFound)
            {
                return NotFound(error.Message);
            }
        }

        return Ok();
    }

    private static TransactionResponse MapTransactionToTransactionResponse(Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.UserId,
            transaction.Description,
            transaction.Amount,
            Enum.Parse<Contracts.Transactions.TransactionType>(transaction.Type.ToString()),
            transaction.CreatedAt,
            transaction.UpdatedAt);
    }
}
