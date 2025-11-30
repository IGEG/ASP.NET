using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Repositories
{
    public class MongoPreferencesRepository : MongoRepository<Preference>
    {
        public MongoPreferencesRepository(IMongoCollection<Preference> collection) : base(collection)
        {
        }
    }
}
