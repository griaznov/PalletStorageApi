using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PalletStorage.WebApi.Infrastructure.Filters;

public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger logger;

    public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
    }
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException)
        {
            logger.LogInformation("Request was cancelled");
            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult(499);
        }
    }
}
