using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public TEntity Get(int id) => Context.Set<TEntity>().Find(id);

        public async Task<List<TEntity>> GetAllAsync() => await Context.Set<TEntity>().ToListAsync();

        public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);


        public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

        public async Task<TEntity> GetAsync(int? id)=> await Context.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

    }
}