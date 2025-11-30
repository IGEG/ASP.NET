using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public class Customer : MongoBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [BsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
        public List<CustomerPreference> Preferences { get; set; } = new List<CustomerPreference>();

        [BsonRepresentation(BsonType.ObjectId)]
        public List<Guid> PromoCodeIds { get; set; } = new List<Guid>();
    }
}