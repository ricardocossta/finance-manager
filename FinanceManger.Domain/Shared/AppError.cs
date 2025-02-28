using FluentResults;

namespace FinanceManger.Domain.Shared;

public class AppError : Error
{
    public AppError(string message, ErrorType errorType) : base(message)
    {
        WithMetadata("ErrorType", errorType);
    }
}
