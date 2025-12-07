using MassTransit;
using Pcf.Administration.Core.Abstractions;
using RAbbitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.DataAccess
{
    public class PromoCodeReceivedConsumer : IConsumer<PromoCodeReceivedEvent>
    {
        private readonly IEmployeePromoCodeService _employeeService;

        public PromoCodeReceivedConsumer(IEmployeePromoCodeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<PromoCodeReceivedEvent> context)
        {
            var message = context.Message;

            if (message.PartnerManagerId.HasValue)
            {
                await _employeeService.UpdateAppliedPromocodesAsync(message.PartnerManagerId.Value);
            }
        }
    }
}
