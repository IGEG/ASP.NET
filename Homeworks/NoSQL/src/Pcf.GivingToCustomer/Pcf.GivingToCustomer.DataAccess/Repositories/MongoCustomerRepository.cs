using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Repositories
{
    public class MongoCustomerRepository : MongoRepository<Customer>
    {
        public MongoCustomerRepository(IMongoCollection<Customer> collection) : base(collection)
        {
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithPreferenceAsync(Guid preferenceId)
        {
            var preference = await _collection.Find(x =>
                x.Preferences.Any(p => p.PreferenceId == preferenceId)).ToListAsync();
            return preference;
        }

        public async Task AddPreferenceToCustomerAsync(Guid customerId, CustomerPreference preference)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.EntityId, customerId);
            var update = Builders<Customer>.Update.Push(x => x.Preferences, preference);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
