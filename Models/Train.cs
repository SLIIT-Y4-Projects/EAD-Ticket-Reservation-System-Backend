/**
 * @file Train.cs
 * @brief Model for Train
 */
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketReservationSystemAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Train
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("trainNumber")]
        public string TrainNumber { get; set; } = String.Empty;

        [BsonElement("departureTime")]
        public DateTime DepartureTime { get; set; }

        [BsonElement("capacity")]
        public int Capacity { get; set; }

        // [BsonElement("reservations")]
        // public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        [BsonElement("status")]
        public string Status { get; set; } = "INACTIVE";
    }
}
