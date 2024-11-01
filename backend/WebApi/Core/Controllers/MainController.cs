using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using WebApi.Core.Messaging;

namespace WebApi.Core.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        protected IActionResult CustomResponse(Result result)
        {
            if (result.Success)
                return NoContent();

            return CommandError(result);
        }

        protected IActionResult CustomResponse<T>(Result<T> result)
        {
            if (result.Success)
                return Ok(result.Value);

            return CommandError(result);
        }

        protected IActionResult ErrorResponse(params string[] errors)
        {
            return BadRequest(ModelError(errors));
        }

        private ObjectResult CommandError(Result result)
        {
            return StatusCode(
                (int)result.ErrorStatusCode,
                ModelError(result.ValidationResult)
            );
        }

        private ValidationProblemDetails ModelError(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                .Select(c => c.ErrorMessage)
                .ToArray();

            return ModelError(errors);
        }

        private ValidationProblemDetails ModelError(string[] errors)
        {
            return new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", errors }
            });
        }
    }
}
