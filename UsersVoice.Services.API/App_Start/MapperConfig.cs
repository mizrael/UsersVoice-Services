using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries.Models;

namespace UsersVoice.Services.API
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
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