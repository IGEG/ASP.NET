using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public class PromoCodeCustomer : MongoBaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PromoCodeId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        // Денормализованные данные для быстрого доступа
        public string PromoCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFullName { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    }
}
