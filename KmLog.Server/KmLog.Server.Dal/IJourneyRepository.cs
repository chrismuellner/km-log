using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public interface IJourneyRepository : IBaseRepository<RefuelAction, RefuelActionDto>
    {
    }
}
