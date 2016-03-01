using System.Threading.Tasks;
using UsersVoice.Infrastructure.Mongo.Commands.Entities;

namespace UsersVoice.Infrastructure.Mongo.Services
{
    public interface ITagSlugFinder
    {
        Task<string> FindSlugAsync(Tag tag);
    }
}