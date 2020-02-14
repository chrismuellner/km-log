using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public interface IRefuelActionRepository : IBaseRepository<RefuelAction, RefuelActionDto>
    {
        Task<IEnumerable<RefuelActionInfoDto>> LoadByCarId(Guid carId);
        Task<IEnumerable<RefuelActionInfoDto>> LoadByCarLicensePlate(string licensePlate);
    }
}
