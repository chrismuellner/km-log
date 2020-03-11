using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public interface IRefuelEntryRepository : IIdentifiableBaseRepository<RefuelEntry, RefuelEntryDto>
    {
        Task<IEnumerable<RefuelEntryInfoDto>> LoadByCarLicensePlate(string licensePlate);

        Task<RefuelEntryInfoDto> LoadLatest(string licensePlate);
    }
}
