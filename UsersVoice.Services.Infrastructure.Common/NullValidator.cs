using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common
{
    public sealed class NullValidator<T> : IValidator<T>
    {
        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(new ValidationResult(null));
        }
    }
}