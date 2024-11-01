using FluentValidation.Results;
using System.Net;

namespace WebApi.Core.Messaging
{
    public class Result<T> : Result
    {
        public T Value { get; set; }
    }

    public class Result
    {
        public ValidationResult ValidationResult { get; set; }
        public HttpStatusCode? ErrorStatusCode { get; set; }

        public bool Success => !ErrorStatusCode.HasValue;
    }
}
