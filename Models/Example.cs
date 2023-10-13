using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketReservationSystemAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Example
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("title")]
        public string Title { get; set; } = String.Empty;

        [BsonElement("content")]
        public string Content { get; set; } = String.Empty;
    }
}
