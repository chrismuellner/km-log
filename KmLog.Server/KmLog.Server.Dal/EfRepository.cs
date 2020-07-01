using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.EF;
using KmLog.Server.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal
{
    public class EfRepository<T> : IRepository<T> where T : IdentifiableBase
    {
        private readonly KmLogContext _context;

        public EfRepository(KmLogContext context)
        {
            _context = context;
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Query() => _context.Set<T>().AsQueryable().AsNoTracking();

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
