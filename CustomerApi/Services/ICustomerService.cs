using CustomerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        public Task AddAsync(Customer customerInfo);
        public Task<List<Customer>> GetAllAsync();
        public Task<Customer> GetAsync(int? customerId);
        public Task Update(Customer customerInfo);
        public Task Remove(Customer customerInfo);
    }
}
