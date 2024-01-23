using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HulkBot
{
    public interface IKnowledgeRepository 
    {
        List<Knowledge> Query(string subject);
    }
    public class KnowledgeRepository : IKnowledgeRepository
    {
        private readonly IMongoCollection<Knowledge> _knowledgeCollection;
        public KnowledgeRepository(IOptions<ChatDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _knowledgeCollection = database.GetCollection<Knowledge>("knowledge");
        }
        
        public List<Knowledge> Query(string subject) 
        {
            var filter = Builders<Knowledge>.Filter.Empty;
            var result = _knowledgeCollection.Find(filter).ToList();
            return result;
        }
    }
}
