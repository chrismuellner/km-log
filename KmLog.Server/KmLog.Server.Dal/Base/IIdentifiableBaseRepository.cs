using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dto.Base;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Dal.Base
{
    public interface IIdentifiableBaseRepository<TEntity, TDto> : IBaseRepository<TEntity, TDto>
        where TEntity : IdentifiableBase
        where TDto : IdentifiableBaseDto
    {
        Task Add(IEnumerable<TDto> entities);

        Task Add(TDto entity);

        Task Delete(Guid id);

        Task<TDto> LoadById(Guid id);

        Task Update(TDto entity);
    }
}
