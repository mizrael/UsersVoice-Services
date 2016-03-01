namespace UsersVoice.Services.Infrastructure.Common.Services
{
    public interface ISlugGenerator
    {
        string GenerateSlug(string phrase);
    }
}
