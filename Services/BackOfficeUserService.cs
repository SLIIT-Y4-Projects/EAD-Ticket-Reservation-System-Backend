/**
 * @file BackOfficeUserService.cs
 * @brief BackOfficeUser service
 */
using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class BackOfficeUserService : IBackOfficeUserService
    {
        private readonly IMongoCollection<BackOfficeUser> _backOfficeUsers;

        // Constructor
        public BackOfficeUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _backOfficeUsers = database.GetCollection<BackOfficeUser>("back_office_users");
        }

        // Create a new BackOfficeUser
        public BackOfficeUser Create(BackOfficeUser backOfficeUser)
        {
            _backOfficeUsers.InsertOne(backOfficeUser);
            return backOfficeUser;
        }

        // Get all BackOfficeUsers
        public List<BackOfficeUser> Get()
        {
            return _backOfficeUsers.Find(backOfficeUser => true).ToList();
        }

        // Get BackOfficeUser by id
        public BackOfficeUser Get(string id)
        {
            return _backOfficeUsers.Find(backOfficeUser => backOfficeUser.Id == id).FirstOrDefault();
        }

        // Get BackOfficeUser by username
        public BackOfficeUser GetByUsername(string username)
        {
            return _backOfficeUsers.Find(backOfficeUser => backOfficeUser.Username == username).FirstOrDefault();
        }

        // Update BackOfficeUser
        public void Update(string id, BackOfficeUser backOfficeUser)
        {
            _backOfficeUsers.ReplaceOne(backOfficeUser => backOfficeUser.Id == id, backOfficeUser);
        }

        // Remove BackOfficeUser
        public void Remove(string id)
        {
            _backOfficeUsers.DeleteOne(backOfficeUser => backOfficeUser.Id == backOfficeUser.Id);
        }
    }
}
