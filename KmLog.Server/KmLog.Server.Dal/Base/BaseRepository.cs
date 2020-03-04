using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;
using KmLog.Server.EF;
using KmLog.Server.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal.Base
{
    public class BaseRepository<TEntity, TDto> : IBaseRepository<TEntity, TDto>
        where TEntity : IdentifiableBase
        where TDto : IdentifiableBaseDto
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

        public async Task<TDto> LoadById(Guid id)
        {
            var entity = await Query()
                .FirstOrDefaultAsync(e => e.Id == id);

            return Mapper.Map<TDto>(entity);
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

        public async Task Add(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();

            Mapper.Map(entity, dto);
        }

        public async Task Add(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();

            Mapper.Map(entities, dtos);
        }

        public async Task Update(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();

            Mapper.Map(entity, dto);
        }

        public async Task Delete(Guid id)
        {
            var dto = await LoadById(id);
            var entity = Mapper.Map<TEntity>(dto);

            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
