using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Abstractions
{
    public interface IEmployeePromoCodeService
    {
        Task UpdateAppliedPromocodesAsync(Guid employeeId);
    }

}
