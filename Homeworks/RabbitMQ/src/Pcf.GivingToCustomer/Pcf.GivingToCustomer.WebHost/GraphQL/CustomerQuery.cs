using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.WebHost.GraphQL
{
    [ExtendObjectType(Name = "Query")]
    public class CustomerQuery
    {
        public async Task<IEnumerable<Customer>> GetCustomers([Service] IRepository<Customer> customerRepository)
        {
            return await customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetCustomer(Guid id,[Service] IRepository<Customer> customerRepository)
        {
            return await customerRepository.GetByIdAsync(id);
        }
    }
}