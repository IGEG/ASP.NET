using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain.Mongo;
using Microsoft.Extensions.Configuration;

namespace Pcf.GivingToCustomer.DataAccess
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("PromoCodeDb");
        }

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("customers");
        public IMongoCollection<Preference> Preferences => _database.GetCollection<Preference>("preferences");
        public IMongoCollection<PromoCode> PromoCodes => _database.GetCollection<PromoCode>("promocodes");
    }
}
