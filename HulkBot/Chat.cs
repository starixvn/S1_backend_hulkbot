using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HulkBot;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public List<Message> Messages { get; set; }
}