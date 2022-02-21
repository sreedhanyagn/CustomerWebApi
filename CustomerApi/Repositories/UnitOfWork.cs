using CustomerAPI.Models;
using System;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICustomerRepository Customers { get; }

        public UnitOfWork(ApplicationDbContext customerDBContext,
            ICustomerRepository customerRepository)
        {
            _context = customerDBContext;

            Customers = customerRepository;
        }
        public async Task<int> Complete() => await _context.SaveChangesAsync();
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
