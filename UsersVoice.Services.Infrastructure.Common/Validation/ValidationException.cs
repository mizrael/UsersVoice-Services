using System;
using System.Collections.Generic;
using System.Linq;

namespace UsersVoice.Services.Infrastructure.Common.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> errors, string message = "")
            : base(message)
        {
            this.Errors = (errors ?? Enumerable.Empty<ValidationError>()).ToArray();
        }
        public ValidationError[] Errors { get; private set; }
    }

    public class ValidationError
    {
        public ValidationError(string message)
        {
            if(string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");
            this.Message = message;
        }
        public string Message { get; private set; }
    }
}