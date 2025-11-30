using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public class Preference : MongoBaseEntity
    {
        public string Name { get; set; }
    }
}
