using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Model.Base;

namespace KmLog.Server.Dal
{
    public interface IRepository<T> where T : IdentifiableBase
    {
        Task Add(IEnumerable<T> entities);

        Task Add(T entity);

        void Delete(T entity);

        IQueryable<T> Query();

        void Update(T entity);
    }
}
