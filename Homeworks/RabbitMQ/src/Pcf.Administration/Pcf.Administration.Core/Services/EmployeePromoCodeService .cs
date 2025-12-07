using Pcf.Administration.Core.Abstractions;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Services
{
    public class EmployeePromoCodeService : IEmployeePromoCodeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeePromoCodeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Обновить количество выданных промокодов
        /// </summary>
        /// <param name="id">Id сотрудника, например <example>451533d5-d8d5-4a11-9c7b-eb9f14e1a32f</example></param>
        public async Task UpdateAppliedPromocodesAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee != null)
            {
                employee.AppliedPromocodesCount++;

                await _employeeRepository.UpdateAsync(employee);
            }
        }
    }
}
