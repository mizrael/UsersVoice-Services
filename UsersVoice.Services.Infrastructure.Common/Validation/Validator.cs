using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common.Validation
{
    public abstract class Validator<T> : IValidator<T>
    {
        private readonly List<ValidationError> _errors;

        protected Validator()
        {
            _errors = new List<ValidationError>();
        }

        public async Task ValidateAsync(T command)
        {
            await this.RunAsync(command);

            var errors = _errors.Where(e => null != e).ToArray();
            if (errors.Any())
                throw new ValidationException(errors);
        }

        protected void AddError(ValidationError error)
        {
            _errors.Add(error);
        }

        protected abstract Task RunAsync(T command);
    }
}