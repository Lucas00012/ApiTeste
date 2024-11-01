using FluentValidation.Results;
using MediatR;
using System.Net;

namespace WebApi.Core.Messaging
{
    public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private ValidationResult ValidationResult;

        public BaseHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected void AdicionarErros(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                ValidationResult.Errors.Add(erro);
            }
        }

        protected Result<T> Success<T>(T value)
        {
            AssertSuccessResult();
            return new Result<T>
            {
                Value = value
            };
        }

        protected Result<T> Error<T>(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            AssertErrorCode(statusCode);
            return new Result<T>
            {
                ValidationResult = ValidationResult,
                ErrorStatusCode = statusCode
            };
        }

        protected Result Success()
        {
            AssertSuccessResult();
            return new Result();
        }

        protected Result Error(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            AssertErrorCode(statusCode);
            return new Result
            {
                ErrorStatusCode = statusCode,
                ValidationResult = ValidationResult
            };
        }

        private void AssertSuccessResult()
        {
            if (!ValidationResult.IsValid)
                throw new InvalidOperationException();
        }

        private void AssertErrorCode(HttpStatusCode statusCode)
        {
            if (statusCode < HttpStatusCode.BadRequest)
                throw new InvalidOperationException();
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
