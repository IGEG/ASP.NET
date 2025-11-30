using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Domain.Mongo
{
    public class PromoCode : MongoBaseEntity
    {
        public string Code { get; set; }
        public string ServiceInfo { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid PartnerId { get; set; }
        public Guid PreferenceId { get; set; } 
        public string PreferenceName { get; set; }
        public List<Guid> CustomerIds { get; set; } = new List<Guid>(); 
    }
}
