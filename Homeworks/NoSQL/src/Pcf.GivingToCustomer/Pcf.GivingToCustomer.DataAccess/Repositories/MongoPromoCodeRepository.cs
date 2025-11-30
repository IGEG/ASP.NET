using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Repositories
{
    public class MongoPromoCodeRepository : MongoRepository<PromoCode>
    {
        public MongoPromoCodeRepository(IMongoCollection<PromoCode> collection) : base(collection)
        {
        }

        public async Task<IEnumerable<PromoCode>> GetByPreferenceIdAsync(Guid preferenceId)
        {
            return await _collection.Find(x => x.PreferenceId == preferenceId).ToListAsync();
        }

        public async Task AddCustomersToPromoCodeAsync(Guid promoCodeId, List<Guid> customerIds)
        {
            var filter = Builders<PromoCode>.Filter.Eq(x => x.Id, promoCodeId);
            var update = Builders<PromoCode>.Update.AddToSetEach(x => x.CustomerIds, customerIds);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
