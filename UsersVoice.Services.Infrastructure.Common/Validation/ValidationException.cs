using System;
using System.Collections.Generic;
using System.Linq;

namespace UsersVoice.Services.Infrastructure.Common
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> errors, string message = "", Exception innerException = null)
            : base(message, innerException)
        {
            this.Errors = (errors ?? Enumerable.Empty<ValidationError>()).ToArray();
        }
        public ValidationError[] Errors { get; private set; }
    }
}