using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAbbitModels
{
    public class PromoCodeReceivedEvent
    {
        public Guid PartnerId { get; set; }
        public string PromoCode { get; set; }
        public string ServiceInfo { get; set; }
        public Guid PreferenceId { get; set; }
        public Guid? PartnerManagerId { get; set; }
    }
}
