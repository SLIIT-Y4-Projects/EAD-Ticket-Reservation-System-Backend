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

        public TravelAgentUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _travelAgentUsers = database.GetCollection<TravelAgentUser>("travel_agent_users");
        }

        public TravelAgentUser Create(TravelAgentUser travelAgentUser)
        {
            _travelAgentUsers.InsertOne(travelAgentUser);
            return travelAgentUser;
        }

        public List<TravelAgentUser> Get()
        {
            return _travelAgentUsers.Find(travelAgentUser => true).ToList();
        }

        public TravelAgentUser Get(string id)
        {
            return _travelAgentUsers.Find(travelAgentUser => travelAgentUser.Id == id).FirstOrDefault();
        }

        public TravelAgentUser GetByUsername(string username)
        {
            return _travelAgentUsers.Find(travelAgentUser => travelAgentUser.Username == username).FirstOrDefault();
        }

        public void Update(string id, TravelAgentUser travelAgentUser)
        {
            _travelAgentUsers.ReplaceOne(travelAgentUser => travelAgentUser.Id == id, travelAgentUser);
        }

        public void Remove(string id)
        {
            _travelAgentUsers.DeleteOne(travelAgentUser => travelAgentUser.Id == travelAgentUser.Id);
        }
    }
}
