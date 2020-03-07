using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using KmLog.Server.Domain;
using KmLog.Server.EF;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal.Base
{
    public class BaseRepository<TEntity, TDto> : IBaseRepository<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected KmLogContext Context { get; }
        protected IMapper Mapper { get; }

        public BaseRepository(KmLogContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected virtual IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<TDto>> LoadAll()
        {
            var entities = await Query()
                .ToListAsync();

            return Mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<PagingResult<TDto>> LoadPaged<TKey>(PagingParameters pagingParameters,
                                                              Expression<Func<TEntity, TKey>> order,
                                                              Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = Query();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var entities = await query
                .OrderByDescending(order)
                .Skip(pagingParameters.ItemsToSkip)
                .Take(pagingParameters.PageSize)
                .ToListAsync();

            return new PagingResult<TDto>
            {
                Count = await query.CountAsync(),
                Items = Mapper.Map<IEnumerable<TDto>>(entities)
            };
        }
    }
}
