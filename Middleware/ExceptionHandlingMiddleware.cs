using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication8.Exceptions;

namespace WebApplication8.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
                        RequestDelegate next,
                        ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(EmailAlreadyExistsException ex)
            {
                _logger.LogWarning(ex, 
                            "Бизнес-ошибка. Method {Method}, Path: {Path}, TraceId {TraceId}",
                            context.Request.Method,
                            context.Request.Path,
                            context.TraceIdentifier);

                await WriteProblemDetailAsync(
                            context,
                            statusCode: (int)HttpStatusCode.Conflict,
                            title: "Conflict",
                            detail: ex.Message);
            }
            catch(OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogInformation(
                            "Запрос был отменен клиентом. Method: {Method}, Path {Path}, TraceId {TraceId}",
                            context.Request.Method,
                            context.Request.Path,
                            context.TraceIdentifier);

                if (!context.Response.HasStarted)
                    context.Response.StatusCode = 499;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,
                            "Необработанное исключение. Method {Method}, Path: {Path}, TraceId {TraceId}",
                            context.Request.Method,
                            context.Request.Path,
                            context.TraceIdentifier);

                await WriteProblemDetailAsync(
                            context,
                            statusCode: (int)HttpStatusCode.InternalServerError,
                            title: "Internal Server Error",
                            detail: "Произошла внутренняя ошибка сервера.");
            }
        }
        private static async Task WriteProblemDetailAsync(
                            HttpContext context,
                            int statusCode,
                            string title,
                            string detail)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };
        }
    }
}
