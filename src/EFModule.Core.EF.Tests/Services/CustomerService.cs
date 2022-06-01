using EFModule.Core.Abstractions.Trackable;
using EFModule.Core.EF.Tests.Models;
using EFModule.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFModule.Core.EF.Tests.Services
{
    public class CustomerService : Service<Customer>, ICustomerService
    {
        private readonly ITrackableRepository<Order> _ordeRepository;

        public CustomerService(
            ITrackableRepository<Customer> customerRepository,
            ITrackableRepository<Order> ordeRepository) : base(customerRepository)
        {
            _ordeRepository = ordeRepository;
        }

        public async Task<IEnumerable<Customer>> CustomersByCompany(string companyName)
        {
            return await Repository
                .Queryable()
                .Where(x => x.CompanyName.Contains(companyName))
                .ToListAsync();
        }

        public async Task<decimal> CustomerOrderTotalByYear(string customerId, int year)
        {
            var customers = await Repository
                .Queryable()
                .Where(c => c.CustomerId == customerId)
                .ToListAsync();
            return customers
                .SelectMany(c => c.Orders.Where(o => o.OrderDate != null && o.OrderDate.Value.Year == year))
                .SelectMany(c => c.OrderDetails)
                .Select(c => c.Quantity * c.UnitPrice)
                .Sum();
        }

        public async Task<IEnumerable<CustomerOrder>> GetCustomerOrder(string country)
        {
            var customers = Repository.Queryable();
            var orders = _ordeRepository.Queryable();

            var query = from c in customers
                        join o in orders on new { a = c.CustomerId, b = c.Country }
                            equals new { a = o.CustomerId, b = country }
                        select new CustomerOrder
                        {
                            CustomerId = c.CustomerId,
                            ContactName = c.ContactName,
                            OrderId = o.OrderId,
                            OrderDate = o.OrderDate
                        };

            return await query.ToListAsync();
        }
    }
}