using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KmLog.Server.Domain;
using KmLog.Server.Dto.Base;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Dal.Base
{
    public interface IBaseRepository<TEntity, TDto>
        where TEntity : IdentifiableBase
        where TDto : IdentifiableBaseDto
    {
        Task Add(IEnumerable<TDto> entities);

        Task Add(TDto entity);

        Task Delete(Guid id);

        Task<TDto> LoadById(Guid id);

        Task<IEnumerable<TDto>> LoadAll();

        Task Update(TDto entity);

        Task<PagingResult<TDto>> LoadPaged<TKey>(PagingParameters pagingParameters,
                                                 Expression<Func<TEntity, TKey>> order,
                                                 Expression<Func<TEntity, bool>> predicate = null);
    }
}
