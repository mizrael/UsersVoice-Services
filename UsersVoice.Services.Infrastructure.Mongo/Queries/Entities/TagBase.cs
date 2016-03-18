namespace UsersVoice.Infrastructure.Mongo.Queries.Entities
{
    /// <summary>
    /// base class for Tags to be used as embedded document
    /// </summary>
    public class TagBase
    {
        public string Text { get; set; }
        public string Slug { get; set; }
    }
}