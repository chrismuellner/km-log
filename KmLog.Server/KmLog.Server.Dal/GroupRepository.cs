using AutoMapper;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.EF;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public class GroupRepository : IdentifiableBaseRepository<Group, GroupDto>, IGroupRepository
    {
        public GroupRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }
    }
}
