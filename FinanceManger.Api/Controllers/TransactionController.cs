using FinanceManger.Application.Transactions.Commands.CreateTransaction;
using FinanceManger.Application.Transactions.Commands.DeleteTransaction;
using FinanceManger.Application.Transactions.Commands.UpdateTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransactions;
using FinanceManger.Contracts.Transactions;
using FinanceManger.Domain.Shared;
using FinanceManger.Domain.Transactions;
using FluentResults;
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

        if (createTransactionResult.IsFailed)
        {
            return Problem(createTransactionResult.Errors);
        }

        return CreatedAtAction(
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
            return Problem(getTransactionResult.Errors);
        }

        return Ok(MapTransactionToTransactionResponse(getTransactionResult.Value));
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var query = new GetTransactionsQuery();

        var transactions = await _mediator.Send(query);

        if (transactions.Count == 0)
        {
            return NoContent();
        }

        return Ok(transactions.Select(MapTransactionToTransactionResponse));
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        var command = new DeleteTransactionCommand(transactionId);
        var deleteTransactionResult = await _mediator.Send(command);

        if (deleteTransactionResult.IsFailed)
        {
            return Problem(deleteTransactionResult.Errors);
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
            return Problem(updateTransactionResult.Errors);
        }

        return Ok();
    }

    private ObjectResult Problem(List<IError> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        var statusCode = errors[0].Metadata["ErrorType"] switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorMessage = errors[0].Message;

        return Problem(statusCode: statusCode, detail: errorMessage);
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
