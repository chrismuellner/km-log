using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.EF;
using KmLog.Server.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : IdentifiableBase
    {
        protected KmLogContext Context { get; }

        public BaseRepository(KmLogContext context)
        {
            Context = context;
        }

        protected IQueryable<T> Query()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public async Task<IEnumerable<T>> LoadAll()
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await Context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Add(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
