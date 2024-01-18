using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HulkBot
{
    public interface IChatRepository
    {
        Task Create(Chat chat);
        Task<Chat> GetAsync(string id);
        Task UpdateAsync(string id, Chat chat);
    }
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<Chat> _chatCollection;
        public ChatRepository(IOptions<ChatDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _chatCollection = database.GetCollection<Chat>("completions");
        }

        public async Task<Chat?> GetAsync(string id) =>
            await _chatCollection.Find(x => x.Id.ToString() == id).FirstOrDefaultAsync();
        public async Task UpdateAsync(string id, Chat chat) =>
            await _chatCollection.ReplaceOneAsync(x => x.Id.ToString() == id, chat);
        public async Task Create(Chat chat) =>
            await _chatCollection.InsertOneAsync(chat);
    }
}
