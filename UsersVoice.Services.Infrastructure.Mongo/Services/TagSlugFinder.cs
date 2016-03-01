using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersVoice.Infrastructure.Mongo.Commands;
using UsersVoice.Services.Infrastructure.Common.Services;
using Tag = UsersVoice.Infrastructure.Mongo.Commands.Entities.Tag;

namespace UsersVoice.Infrastructure.Mongo.Services
{
    public class TagSlugFinder : ITagSlugFinder
    {
        private readonly ISlugGenerator _slugGenerator;
        private readonly ICommandsDbContext _commandsDb;

        public TagSlugFinder(ISlugGenerator slugGenerator, ICommandsDbContext commandsDb)
        {
            if (slugGenerator == null) throw new ArgumentNullException("slugGenerator");
            if (commandsDb == null) throw new ArgumentNullException("commandsDb");
            _slugGenerator = slugGenerator;
            _commandsDb = commandsDb;
        }

        public async Task<string> FindSlugAsync(Tag tag)
        {
            if (null == tag)
                throw new ArgumentNullException("tag");
            if (string.IsNullOrWhiteSpace(tag.Text))
                throw new ArgumentException("tag text must be non empty");
            if (Guid.Empty == tag.Id)
                throw new ArgumentException("tag id must be non empty");

            var baseSlug = _slugGenerator.GenerateSlug(tag.Text);
         
            tag.Slug = string.Empty; 

            var index = 0;
            while (true)
            {
                var slug = (0 == index) ? baseSlug : string.Format("{0}-{1}", baseSlug, index);
                var otherTag = await _commandsDb.Tags.Find(t => t.Id != tag.Id && t.Slug == slug).FirstOrDefaultAsync();
                if (null == otherTag)
                {
                    tag.Slug = slug; 
                    break;
                }

                index++;

                if (index > 100)
                    return string.Empty;
            }

            return tag.Slug;
        }
    }
}