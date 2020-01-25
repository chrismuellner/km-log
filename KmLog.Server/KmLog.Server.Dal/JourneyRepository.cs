using KmLog.Server.Dal.Base;
using KmLog.Server.EF;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public class JourneyRepository : BaseRepository<Journey>, IJourneyRepository
    {
        public JourneyRepository(KmLogContext context) : base(context)
        { }
    }
}
