using AutoMapper;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries.Models;

namespace UsersVoice.Services.API
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TagBase, Common.CQRS.Queries.TagBase>();

                cfg.CreateMap<User, UserArchiveItem>();
                cfg.CreateMap<User, UserDetails>();

                cfg.CreateMap<Area, AreaArchiveItem>();
                cfg.CreateMap<Area, AreaDetails>();

                cfg.CreateMap<Idea, IdeaArchiveItem>();
                cfg.CreateMap<Idea, IdeaDetails>();

                cfg.CreateMap<IdeaComment, IdeaCommentArchiveItem>();
            });
        }
    }
}