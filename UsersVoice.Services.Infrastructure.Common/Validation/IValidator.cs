using System.Threading.Tasks;

namespace UsersVoice.Services.Infrastructure.Common
{
    public interface IValidator<in TCommand>
    {
        Task<ValidationResult> ValidateAsync(TCommand value);
    }
}