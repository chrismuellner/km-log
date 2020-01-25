using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        protected IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<TDto>> LoadAll()
        {
            var entities = await Context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            return Mapper.Map<IEnumerable<TDto>>(entities);
        }

        public async Task<TDto> GetById(Guid id)
        {
            var entity = await Context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            return Mapper.Map<TDto>(entity);
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
            var dto = await GetById(id);
            var entity = Mapper.Map<TEntity>(dto);
            
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
