using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HulkBot
{
    public class ChatService
    {
        private readonly IMongoCollection<Chat> _chatCollection;
        public ChatService(IOptions<ChatDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _chatCollection = database.GetCollection<Chat>(settings.Value.CompletionsCollectionName);
        }

        public async Task<Chat?> GetAsync(string id) =>
            await _chatCollection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        public async Task Create(Chat chat) =>
            await _chatCollection.InsertOneAsync(chat);
    }
}
