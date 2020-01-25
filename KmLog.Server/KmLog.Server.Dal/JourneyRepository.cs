using AutoMapper;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.EF;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public class JourneyRepository : BaseRepository<RefuelAction, RefuelActionDto>, IJourneyRepository
    {
        public JourneyRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }
    }
}
