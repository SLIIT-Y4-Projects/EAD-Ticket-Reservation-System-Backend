/**
 * @file TrainService.cs
 * @brief Implementation file for Train service
 */
using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class TrainService : ITrainService
    {
        private readonly IMongoCollection<Train> _trains;

        // Constructor
        public TrainService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _trains = database.GetCollection<Train>("trains");
        }

        // Activate a train
        public void Activate(string id)
        {
            var filter = Builders<Train>.Filter.Eq("Id", id);
            var update = Builders<Train>.Update.Set("Status", "ACTIVE");
            _trains.UpdateOne(filter, update);
        }

        // Cancel a train
        public void Cancel(string id)
        {
            var filter = Builders<Train>.Filter.Eq("Id", id);
            var update = Builders<Train>.Update.Set("Status", "CANCELLED");
            _trains.UpdateOne(filter, update);
        }

        // Create a train
        public Train Create(Train train)
        {
            _trains.InsertOne(train);
            return train;
        }

        // Get all trains
        public List<Train> Get()
        {
            return _trains.Find(train => true).ToList();
        }

        // Get a train by id
        public Train Get(string id)
        {
            return _trains.Find(train => train.Id == id).FirstOrDefault();
        }

        // Get all trains by user id
        public void Remove(string id)
        {
            _trains.DeleteOne(train => train.Id == train.Id);
        }

        // Update a train
        public void Update(string id, Train train)
        {
            _trains.ReplaceOne(train => train.Id == id, train);
        }
    }
}
