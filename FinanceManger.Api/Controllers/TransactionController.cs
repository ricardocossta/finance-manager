using FinanceManger.Application.Transactions.Commands.CreateTransaction;
using FinanceManger.Application.Transactions.Commands.DeleteTransaction;
using FinanceManger.Application.Transactions.Commands.UpdateTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransaction;
using FinanceManger.Application.Transactions.Queries.GetTransactions;
using FinanceManger.Contracts.Transactions;
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
            : CreatedAtAction(nameof(GetTransaction), new TransactionResponse(
                createTransactionResult.Value.Id,
                request.UserId,
                request.Description,
                request.Amount,
                request.Type));
    }

    [HttpGet("transactionId")]
    public async Task<IActionResult> GetTransaction(Guid transactionId)
    {
        var query = new GetTransactionQuery(transactionId);

        var getTransactionResult = await _mediator.Send(query);

        return getTransactionResult.IsFailed
            ? NotFound(getTransactionResult.Errors[0].Message)
            : Ok(new TransactionResponse(
                getTransactionResult.Value.Id,
                getTransactionResult.Value.UserId,
                getTransactionResult.Value.Description,
                getTransactionResult.Value.Amount,
                Enum.Parse<TransactionType>(getTransactionResult.Value.Type.ToString())));
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

        return Ok(getTransactionsResult.Select(t => new TransactionResponse(
            t.Id,
            t.UserId,
            t.Description,
            t.Amount,
            Enum.Parse<TransactionType>(t.Type.ToString()))));
    }

    [HttpDelete("transactionId")]
    public async Task<IActionResult> DeleteTransaction(Guid transactionId)
    {
        var command = new DeleteTransactionCommand(transactionId);
        var deleteTransactionResult = await _mediator.Send(command);

        return deleteTransactionResult.IsFailed
            ? NotFound(deleteTransactionResult.Errors[0].Message)
            : NoContent();
    }

    [HttpPut("transactionId")]
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

        return updateTransactionResult.IsFailed
            ? Problem()
            : Ok();
    }
}
