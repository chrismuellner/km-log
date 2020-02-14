using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<RefuelActionInfoDto>> LoadByCarId(Guid carId)
        {
            var refuelActions = await Query()
                .Where(ra => ra.CarId == carId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<RefuelActionInfoDto>>(refuelActions);
        }

        public async Task<IEnumerable<RefuelActionInfoDto>> LoadByCarLicensePlate(string licensePlate)
        {
            var refuelActions = await Query()
                .Where(ra => ra.Car.LicensePlate == licensePlate)
                .ToListAsync();

            return Mapper.Map<IEnumerable<RefuelActionInfoDto>>(refuelActions);
        }

        protected override IQueryable<RefuelAction> Query()
        {
            return base.Query()
                .Include(ra => ra.Car);
        }
    }
}
