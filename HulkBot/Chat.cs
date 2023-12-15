using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HulkBot;

public class Chat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; private set; }
    public IList<Message> Messages { get; private set; }
    public Chat()
    {
        Id = ObjectId.GenerateNewId();
    }

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