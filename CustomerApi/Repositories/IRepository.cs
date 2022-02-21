using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int? Id);
        void Add(TEntity entity);
        void Remove(TEntity entity);
        public void Update(TEntity entity);
    }
}
