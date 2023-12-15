using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HulkBot;

public class Chat(ObjectId id)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; private set; } = id;
    public IList<Message> Messages { get; private set; }

    public void InitialMessage(int topic) 
    {
        Messages = new List<Message>
        {
            BaseMessage.Create(topic)
        };
    }
    public void Append(Message message) 
    {
        Messages.Add(message);
    }
}