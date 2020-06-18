using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KmLog.Server.Dto.Base;
using KmLog.Server.EF;
using KmLog.Server.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal.Base
{
    public class IdentifiableBaseRepository<TEntity, TDto> : BaseRepository<TEntity, TDto>, IIdentifiableBaseRepository<TEntity, TDto>
        where TEntity : IdentifiableBase
        where TDto : IdentifiableBaseDto
    {
        public IdentifiableBaseRepository(KmLogContext context, IMapper mapper)
            : base(context, mapper)
        { }

        public async Task<TDto> LoadById(Guid id)
        {
            var entity = await Query()
                .FirstOrDefaultAsync(e => e.Id == id);

            return Mapper.Map<TDto>(entity);
        }

        public async Task Add(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            await Context.Set<TEntity>().AddAsync(entity);

            Mapper.Map(entity, dto);
        }

        public async Task Add(IEnumerable<TDto> dtos)
        {
            var entities = Mapper.Map<IEnumerable<TEntity>>(dtos);

            await Context.Set<TEntity>().AddRangeAsync(entities);

            Mapper.Map(entities, dtos);
        }

        public void Update(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);

            Context.Entry(entity).State = EntityState.Modified;

            Mapper.Map(entity, dto);
        }

        public async Task Delete(Guid id)
        {
            var dto = await LoadById(id);
            var entity = Mapper.Map<TEntity>(dto);

            Context.Set<TEntity>().Remove(entity);
        }
    }
}
