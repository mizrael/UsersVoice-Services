using System;

namespace UsersVoice.Services.Infrastructure.Common
{
    public class ValidationError
    {
        public ValidationError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException("message");
            this.Message = message;
        }
        public string Message { get; private set; }
    }
}