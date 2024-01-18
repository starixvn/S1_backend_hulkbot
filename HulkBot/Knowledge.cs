using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HulkBot
{
    public class Knowledge
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; private set; }

        public string Subject { get; set; }
        public string Message { get; set; }
        public string Issue { get; set; }
        public string Solution { get; set; }
    }
}
