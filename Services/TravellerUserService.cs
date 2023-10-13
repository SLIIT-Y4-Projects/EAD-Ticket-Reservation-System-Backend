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

        public TravellerUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _travellerUsers = database.GetCollection<TravellerUser>("traveller_users");
        }

        public void Activate(string id)
        {
            var filter = Builders<TravellerUser>.Filter.Eq("Id", id);
            var update = Builders<TravellerUser>.Update.Set("Status", "ACTIVE");
            _travellerUsers.UpdateOne(filter, update);
        }

        public TravellerUser Create(TravellerUser travelAgentUser)
        {
            _travellerUsers.InsertOne(travelAgentUser);
            return travelAgentUser;
        }

        public void Deactivate(string id)
        {
            var filter = Builders<TravellerUser>.Filter.Eq("Id", id);
            var update = Builders<TravellerUser>.Update.Set("Status", "INACTIVE");
            _travellerUsers.UpdateOne(filter, update);
        }

        public List<TravellerUser> Get()
        {
            return _travellerUsers.Find(travellerUser => true).ToList();
        }

        public TravellerUser Get(string id)
        {
            return _travellerUsers.Find(travellerUser => travellerUser.Id == id).FirstOrDefault();
        }

        public TravellerUser GetByUsername(string username)
        {
            return _travellerUsers.Find(travellerUser => travellerUser.Username == username).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _travellerUsers.DeleteOne(travellerUser => travellerUser.Id == travellerUser.Id);
        }

        public void Update(string id, TravellerUser travellerUser)
        {
            _travellerUsers.ReplaceOne(travellerUser => travellerUser.Id == id, travellerUser);
        }
    }
}
