using System.Linq;
using UsersVoice.Infrastructure.Mongo.Queries.Entities;
using UsersVoice.Services.API.CQRS.Queries.Models;

namespace UsersVoice.Services.API
{
    public class MapperConfig
    {
        public static void RegisterMappers()
        {
            AutoMapper.Mapper.CreateMap<User, UserArchiveItem>()
                .ForMember(i => i.Tags, e => e.ResolveUsing(i => i.Tags.Select(t => t.Slug).ToArray()));
            AutoMapper.Mapper.CreateMap<User, UserDetails>()
                .ForMember(i => i.Tags, e => e.ResolveUsing(i => i.Tags.Select(t => t.Slug).ToArray()));

            AutoMapper.Mapper.CreateMap<Area, AreaArchiveItem>();
            AutoMapper.Mapper.CreateMap<Area, AreaDetails>();

            AutoMapper.Mapper.CreateMap<Idea, IdeaArchiveItem>()
                .ForMember(i => i.Tags, e => e.ResolveUsing(i => i.Tags.Select(t => t.Slug).ToArray()));
            AutoMapper.Mapper.CreateMap<Idea, IdeaDetails>()
                 .ForMember(i => i.Tags, e => e.ResolveUsing(i => i.Tags.Select(t => t.Slug).ToArray()));

            AutoMapper.Mapper.CreateMap<IdeaComment, IdeaCommentArchiveItem>();

            AutoMapper.Mapper.CreateMap<Tag, TagArchiveItem>();
        }
    }
}