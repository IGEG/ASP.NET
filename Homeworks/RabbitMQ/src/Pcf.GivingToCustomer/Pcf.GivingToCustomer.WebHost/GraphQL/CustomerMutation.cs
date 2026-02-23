using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.GivingToCustomer.WebHost.GraphQL.InputTypes;

namespace Pcf.GivingToCustomer.WebHost.GraphQL
{
    [ExtendObjectType(Name = "Mutation")]
    public class CustomerMutation
    {
        public async Task<Customer> CreateCustomer( CreateCustomerInput input, [Service] IRepository<Customer> customerRepository, [Service] IRepository<Preference> preferenceRepository)
        {
            // Получаем предпочтения из БД
            var preferences = await preferenceRepository.GetRangeByIdsAsync(input.PreferenceIds);

            // Создаем модель запроса для маппера
            var createRequest = new CreateOrEditCustomerRequest
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                PreferenceIds = input.PreferenceIds
            };

            var customer = CustomerMapper.MapFromModel(createRequest, preferences);

            await customerRepository.AddAsync(customer);

            return customer;
        }

        public async Task<Customer> UpdateCustomer( UpdateCustomerInput input, [Service] IRepository<Customer> customerRepository, [Service] IRepository<Preference> preferenceRepository)
        {
            var customer = await customerRepository.GetByIdAsync(input.Id);

            if (customer == null)
                throw new GraphQLException($"Customer с id {input.Id} не найден");

            var preferences = await preferenceRepository.GetRangeByIdsAsync(input.PreferenceIds);

            // Создаем модель запроса для маппера
            var updateRequest = new CreateOrEditCustomerRequest
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                PreferenceIds = input.PreferenceIds
            };

            CustomerMapper.MapFromModel(updateRequest, preferences, customer);

            await customerRepository.UpdateAsync(customer);

            return customer;
        }

        public async Task<bool> DeleteCustomer( DeleteCustomerInput input, [Service] IRepository<Customer> customerRepository)
        {
            var customer = await customerRepository.GetByIdAsync(input.Id);

            if (customer == null)
                throw new GraphQLException($"Customer с id {input.Id} не найден");

            await customerRepository.DeleteAsync(customer);

            return true;
        }
    }
}