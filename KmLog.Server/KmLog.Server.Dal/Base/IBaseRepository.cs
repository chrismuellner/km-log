using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KmLog.Server.Domain;

namespace KmLog.Server.Dal.Base
{
    public interface IBaseRepository<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        Task<IEnumerable<TDto>> LoadAll();

        Task<PagingResult<TDto>> LoadPaged<TKey>(PagingParameters pagingParameters,
                                                 Expression<Func<TEntity, TKey>> order,
                                                 Expression<Func<TEntity, bool>> predicate = null);
    }
}
