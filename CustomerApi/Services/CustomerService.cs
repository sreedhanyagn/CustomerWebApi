
using CustomerAPI.Models;
using CustomerAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
       private readonly IUnitOfWork _unitOfWork;       
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(Customer customerInfo)
        {        
            
            _unitOfWork.Customers.Add(customerInfo);
            await _unitOfWork.Complete();
        }
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _unitOfWork.Customers.GetAllAsync();
        }
        public async Task<Customer> GetAsync(int? customerId)
        {
            return await _unitOfWork.Customers.GetAsync(customerId);
        }

        public async Task Update(Customer customerInfo)
        {
           
            _unitOfWork.Customers.Update(customerInfo);
            await _unitOfWork.Complete();
        }
        public async Task Remove(Customer customerInfo)
        {
            _unitOfWork.Customers.Remove(customerInfo);
            await _unitOfWork.Complete();
        }
    }
}
