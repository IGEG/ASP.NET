using MassTransit;
using Pcf.GivingToCustomer.Core.Abstractions;
using RAbbitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess
{
    public class PromoCodeReceivedConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromoCodeReceivedConsumer(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            await _promoCodeService.GivePromoCodesToCustomersWithPreferenceAsync(context.Message);
        }
    }
}
