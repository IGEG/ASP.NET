using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public abstract class MongoBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid EntityId { get; set; } = Guid.NewGuid();
    }
}
