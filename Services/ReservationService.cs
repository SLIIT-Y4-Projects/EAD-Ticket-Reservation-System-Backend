/**
 * @file ReservationService.cs
 * @brief Implementation file for Reservation service
 */
using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservations;

        // Constructor
        public ReservationService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _reservations = database.GetCollection<Reservation>("reservations");
        }

        // Cancel a reservation
        public void Cancel(string id)
        {
            var filter = Builders<Reservation>.Filter.Eq("Id", id);
            var update = Builders<Reservation>.Update.Set("Status", "CANCELLED");
            _reservations.UpdateOne(filter, update);
        }

        // Confirm a reservation
        public Reservation Create(Reservation reservation)
        {
            _reservations.InsertOne(reservation);
            return reservation;
        }

        // Get all reservations
        public List<Reservation> Get()
        {
            return _reservations.Find(reservation => true).ToList();
        }

        // Get a reservation by id
        public Reservation Get(string id)
        {
            return _reservations.Find(reservation => reservation.Id == id).FirstOrDefault();
        }

        // Get all reservations by user id
        public void Remove(string id)
        {
            _reservations.DeleteOne(reservation => reservation.Id == reservation.Id);
        }

        // Update a reservation
        public void Update(string id, Reservation reservation)
        {
            _reservations.ReplaceOne(reservation => reservation.Id == id, reservation);
        }
    }
}
