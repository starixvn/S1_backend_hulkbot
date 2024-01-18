using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HulkBot
{
    public interface IKonwledgeRepository 
    {
        List<Knowledge> Query(string subject);
    }
    public class KonwledgeRepository : IKonwledgeRepository
    {
        private readonly IMongoCollection<Knowledge> _knowledgeCollection;
        public KonwledgeRepository(IOptions<ChatDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _knowledgeCollection = database.GetCollection<Knowledge>("knowledge");
        }
        
        public List<Knowledge> Query(string subject) 
        {
            //var filter = Builders<Knowledge>.Filter.Empty;
            var filter = Builders<Knowledge>.Filter.Eq(k => k.Subject, subject);
            var result = _knowledgeCollection.Find(filter).ToList();
            return result;
        }
    }
}
