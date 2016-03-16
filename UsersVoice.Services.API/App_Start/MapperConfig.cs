using System.Linq;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries.Models;
using TagBase = UsersVoice.Infrastructure.Mongo.Queries.Entities.TagBase;

namespace UsersVoice.Services.API
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
            AutoMapper.Mapper.CreateMap<TagBase, CQRS.Queries.Models.TagBase>();

            AutoMapper.Mapper.CreateMap<User, UserArchiveItem>();
            AutoMapper.Mapper.CreateMap<User, UserDetails>();

            AutoMapper.Mapper.CreateMap<Area, AreaArchiveItem>();
            AutoMapper.Mapper.CreateMap<Area, AreaDetails>();

            AutoMapper.Mapper.CreateMap<Idea, IdeaArchiveItem>();
            AutoMapper.Mapper.CreateMap<Idea, IdeaDetails>();

            AutoMapper.Mapper.CreateMap<IdeaComment, IdeaCommentArchiveItem>();

            AutoMapper.Mapper.CreateMap<Tag, TagArchiveItem>();
        }
    }
}