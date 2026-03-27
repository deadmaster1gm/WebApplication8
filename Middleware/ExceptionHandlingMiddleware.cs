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
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(EmailAlreadyExistsException ex)
            {
                _logger.LogWarning(ex,
                        "Бизнес-ошибка. Method: {Method}, Path: {Path}, TraceId: {TraceId}",
                        context.Request.Method,
                        context.Request.Path,
                        context.TraceIdentifier);

                await WriteProblemDetailAsync(
                                context,
                                StatusCode: (int)HttpStatusCode.Conflict,
                                title: "Conflict",
                                detail: ex.Message);
            }
        }

        private static async Task WriteProblemDetailAsync(
                            HttpContext context,
                            int StatusCode,
                            string title,
                            string detail)
        {
            if (context.Response.HasStarted)
                        return;

            context.Response.Clear();
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
