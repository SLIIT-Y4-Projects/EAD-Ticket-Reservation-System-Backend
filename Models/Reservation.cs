/**
 * @file Reservation.cs
 * @brief Model for Reservation
 */
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketReservationSystemAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("reservationDate")]
        public string ReservationDate { get; set; } = String.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "ACTIVE";

        [BsonElement("passengerId")]
        public string PassengerId { get; set; } = String.Empty;

        [BsonElement("trainId")]
        public string TrainId { get; set; } = String.Empty;
    }
}
