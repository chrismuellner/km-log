using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Dal.Base
{
    public interface IBaseRepository<T> where T : IdentifiableBase
    {
        Task Add(IEnumerable<T> entities);

        Task Add(T entity);

        Task Delete(Guid id);

        Task<T> GetById(Guid id);

        Task<IEnumerable<T>> LoadAll();

        Task Update(T entity);
    }
}
