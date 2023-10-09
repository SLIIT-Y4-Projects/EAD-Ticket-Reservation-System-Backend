using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class BackOfficeUserService : IBackOfficeUserService
    {
        private readonly IMongoCollection<BackOfficeUser> _backOfficeUsers;

        public BackOfficeUserService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _backOfficeUsers = database.GetCollection<BackOfficeUser>("back_office_users");
        }

        public BackOfficeUser Create(BackOfficeUser backOfficeUser)
        {
            _backOfficeUsers.InsertOne(backOfficeUser);
            return backOfficeUser;
        }

        public List<BackOfficeUser> Get()
        {
            return _backOfficeUsers.Find(backOfficeUser => true).ToList();
        }

        public BackOfficeUser Get(string id)
        {
            return _backOfficeUsers.Find(backOfficeUser => backOfficeUser.Id == id).FirstOrDefault();
        }

        public void Update(string id, BackOfficeUser backOfficeUser)
        {
            _backOfficeUsers.ReplaceOne(backOfficeUser => backOfficeUser.Id == id, backOfficeUser);
        }

        public void Remove(string id)
        {
            _backOfficeUsers.DeleteOne(backOfficeUser => backOfficeUser.Id == backOfficeUser.Id);
        }
    }
}
