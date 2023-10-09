using MongoDB.Driver;
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public class ExampleService : IExampleService
    {

        private readonly IMongoCollection<Example> _examples;

        public ExampleService(ITicketReservationDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _examples = database.GetCollection<Example>("examples");
        }

        public Example Create(Example example)
        {
            _examples.InsertOne(example);
            return example;
        }

        public List<Example> Get()
        {
            return _examples.Find(example => true).ToList();
        }

        public Example Get(string id)
        {
            return _examples.Find(example => example.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _examples.DeleteOne(example => example.Id == id);
        }

        public void Update(string id, Example example)
        {
            _examples.ReplaceOne(example => example.Id == id, example);
        }
    }
}
