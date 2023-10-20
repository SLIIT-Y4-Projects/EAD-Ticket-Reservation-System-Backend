/**
 * @file TravellerUserService.cs
 * @brief Implementation file for TravellerUser service
 */
using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class TravellerUserService : ITravellerUserService
    {
        private readonly IMongoCollection<TravellerUser> _travellerUsers;

        // Constructor
        public TravellerUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _travellerUsers = database.GetCollection<TravellerUser>("traveller_users");
        }

        // Activate a traveller user
        public void Activate(string id)
        {
            var filter = Builders<TravellerUser>.Filter.Eq("Id", id);
            var update = Builders<TravellerUser>.Update.Set("Status", "ACTIVE");
            _travellerUsers.UpdateOne(filter, update);
        }

        // Create a traveller user
        public TravellerUser Create(TravellerUser travelAgentUser)
        {
            _travellerUsers.InsertOne(travelAgentUser);
            return travelAgentUser;
        }

        // Deactivate a traveller user
        public void Deactivate(string id)
        {
            var filter = Builders<TravellerUser>.Filter.Eq("Id", id);
            var update = Builders<TravellerUser>.Update.Set("Status", "INACTIVE");
            _travellerUsers.UpdateOne(filter, update);
        }

        // Get all traveller users
        public List<TravellerUser> Get()
        {
            return _travellerUsers.Find(travellerUser => true).ToList();
        }

        // Get a traveller user by id
        public TravellerUser Get(string id)
        {
            return _travellerUsers.Find(travellerUser => travellerUser.Id == id).FirstOrDefault();
        }

        // Get a traveller user by username
        public TravellerUser GetByUsername(string username)
        {
            return _travellerUsers.Find(travellerUser => travellerUser.Username == username).FirstOrDefault();
        }

        // Update a traveller user
        public void Remove(string id)
        {
            _travellerUsers.DeleteOne(travellerUser => travellerUser.Id == travellerUser.Id);
        }

        // Update a traveller user
        public void Update(string id, TravellerUser travellerUser)
        {
            _travellerUsers.ReplaceOne(travellerUser => travellerUser.Id == id, travellerUser);
        }
    }
}
