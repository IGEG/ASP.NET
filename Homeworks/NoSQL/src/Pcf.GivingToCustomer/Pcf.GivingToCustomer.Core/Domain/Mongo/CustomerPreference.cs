using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public class CustomerPreference
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceName { get; set; }
    }
}
