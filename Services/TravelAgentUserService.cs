/**
 * @file TravelAgentUserService.cs
 * @brief Implementation file for TravelAgentUser service
 */
using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class TravelAgentUserService : ITravelAgentUserService
    {
        private readonly IMongoCollection<TravelAgentUser> _travelAgentUsers;

        // Constructor
        public TravelAgentUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _travelAgentUsers = database.GetCollection<TravelAgentUser>("travel_agent_users");
        }

        // Create a travel agent user
        public TravelAgentUser Create(TravelAgentUser travelAgentUser)
        {
            _travelAgentUsers.InsertOne(travelAgentUser);
            return travelAgentUser;
        }

        // Get all travel agent users
        public List<TravelAgentUser> Get()
        {
            return _travelAgentUsers.Find(travelAgentUser => true).ToList();
        }

        // Get a travel agent user by id
        public TravelAgentUser Get(string id)
        {
            return _travelAgentUsers.Find(travelAgentUser => travelAgentUser.Id == id).FirstOrDefault();
        }

        // Get a travel agent user by username
        public TravelAgentUser GetByUsername(string username)
        {
            return _travelAgentUsers.Find(travelAgentUser => travelAgentUser.Username == username).FirstOrDefault();
        }

        // Update a travel agent user
        public void Update(string id, TravelAgentUser travelAgentUser)
        {
            _travelAgentUsers.ReplaceOne(travelAgentUser => travelAgentUser.Id == id, travelAgentUser);
        }

        // Delete a travel agent user
        public void Remove(string id)
        {
            _travelAgentUsers.DeleteOne(travelAgentUser => travelAgentUser.Id == travelAgentUser.Id);
        }
    }
}
