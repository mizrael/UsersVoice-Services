using AutoMapper;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.TagAPI.CQRS.Queries.Models;
using TagBase = UsersVoice.Infrastructure.Mongo.Queries.Entities.TagBase;

namespace UsersVoice.Services.TagAPI
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TagBase, Common.CQRS.Queries.TagBase>();
                cfg.CreateMap<Tag, TagArchiveItem>();
            });
        }
    }
}