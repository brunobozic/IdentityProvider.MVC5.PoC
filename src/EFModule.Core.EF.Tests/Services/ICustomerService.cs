using EFModule.Core.EF.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFModule.Core.EF.Tests.Services
{
    public interface ICustomerService
    {
        Task<decimal> CustomerOrderTotalByYear(string customerId, int year);

        Task<IEnumerable<Customer>> CustomersByCompany(string companyName);

        Task<IEnumerable<CustomerOrder>> GetCustomerOrder(string country);
    }
}