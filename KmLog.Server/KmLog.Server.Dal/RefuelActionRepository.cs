using System.Linq;
using AutoMapper;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.EF;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.Dal
{
    public class RefuelActionRepository : BaseRepository<RefuelAction, RefuelActionDto>, IRefuelActionRepository
    {
        public RefuelActionRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }

        protected override IQueryable<RefuelAction> Query()
        {
            return base.Query()
                .Include(ra => ra.Car);
        }
    }
}
