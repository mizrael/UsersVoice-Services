using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common.Validation
{
    public interface IValidator<in T>
    {
        Task ValidateAsync(T value);
    }
}
