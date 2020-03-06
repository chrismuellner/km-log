using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public interface ICarRepository : IBaseRepository<Car, CarDto>
    {
        Task<CarDto> LoadByLicensePlate(string licensePlate);

        Task<IEnumerable<CarDto>> LoadByUser(Guid userId);
    }
}
