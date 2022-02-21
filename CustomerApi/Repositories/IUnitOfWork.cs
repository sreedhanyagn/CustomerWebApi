using System;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        public Task<int> Complete();
    }
}
