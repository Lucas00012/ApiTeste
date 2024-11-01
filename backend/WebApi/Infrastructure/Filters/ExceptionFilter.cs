using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Infrastructure.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(IHostEnvironment environment, ILogger<ExceptionFilter> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogCritical(ex, ex.Message);

            var errorModel = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", new[] { "Houve um erro inesperado. Tente novamente mais tarde" } }
            })
            {
                Title = "Erro interno do servidor"
            };

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(errorModel);
        }
    }
}
