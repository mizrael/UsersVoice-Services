using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common.Validation
{
    public sealed class NullValidator<T> : IValidator<T>
    {
        public Task ValidateAsync(T value)
        {
            return Task.CompletedTask;
        }
    }
}