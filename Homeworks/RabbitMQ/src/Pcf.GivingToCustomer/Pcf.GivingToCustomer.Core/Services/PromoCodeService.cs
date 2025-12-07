using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using RAbbitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PromoCodeService(IRepository<PromoCode> promoCodesRepository, IRepository<Preference> preferencesRepository,IRepository<Customer> customersRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        public async Task GivePromoCodesToCustomersWithPreferenceAsync(PromoCodeReceivedEvent promoEvent)
        {
            var preference = await _preferencesRepository.GetByIdAsync(promoEvent.PreferenceId);
            if (preference == null) return;

            var customers = await _customersRepository.GetWhere(d => d.Preferences.Any(x => x.Preference.Id == preference.Id));
            if (!customers.Any()) return;

            var promoCode = new PromoCode
            {
                Id = Guid.NewGuid(),
                PartnerId = promoEvent.PartnerId,
                Code = promoEvent.PromoCode,
                ServiceInfo = promoEvent.ServiceInfo,
                BeginDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Preference = preference,
                PreferenceId = preference.Id
            };

            promoCode.Customers = new List<PromoCodeCustomer>();

            foreach (var customer in customers)
            {
                promoCode.Customers.Add(new PromoCodeCustomer()
                {
                    CustomerId = customer.Id,
                    Customer = customer,
                    PromoCodeId = promoCode.Id,
                    PromoCode = promoCode
                });
            }

            await _promoCodesRepository.AddAsync(promoCode);
        }
    }
}
