using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dto.Base;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Dal.Base
{
    public interface IBaseRepository<TEntity, TDTO> 
        where TEntity : IdentifiableBase
        where TDTO : IdentifiableBaseDto
    {
        Task Add(IEnumerable<TDTO> entities);

        Task Add(TDTO entity);

        Task Delete(Guid id);

        Task<TDTO> LoadById(Guid id);

        Task<IEnumerable<TDTO>> LoadAll();

        Task Update(TDTO entity);
    }
}
