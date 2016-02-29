using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common
{
    public abstract class Validator<TCommand> : IValidator<TCommand>
    {
        private readonly List<ValidationError> _errors;

        protected Validator()
        {
            _errors = new List<ValidationError>();
        }

        public async Task<ValidationResult> ValidateAsync(TCommand command)
        {
            await this.RunAsync(command);

            var errors = _errors.Where(e => null != e).ToArray();
            var result = new ValidationResult(errors);

            return result;
        }

        protected void AddError(ValidationError error)
        {
            _errors.Add(error);
        }

        protected abstract Task RunAsync(TCommand command);
    }
}