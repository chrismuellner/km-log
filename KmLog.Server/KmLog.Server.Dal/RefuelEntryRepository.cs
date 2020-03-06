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
    public class RefuelEntryRepository : BaseRepository<RefuelEntry, RefuelEntryDto>, IRefuelEntryRepository
    {
        public RefuelEntryRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }

        public async Task<IEnumerable<RefuelEntryInfoDto>> LoadByCarId(Guid carId)
        {
            var refuelEntries = await Query()
                .Where(ra => ra.CarId == carId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<RefuelEntryInfoDto>>(refuelEntries);
        }

        public async Task<IEnumerable<RefuelEntryInfoDto>> LoadByCarLicensePlate(string licensePlate)
        {
            var refuelEntries = await Query()
                .Where(ra => ra.Car.LicensePlate == licensePlate)
                .ToListAsync();

            return Mapper.Map<IEnumerable<RefuelEntryInfoDto>>(refuelEntries);
        }

        protected override IQueryable<RefuelEntry> Query()
        {
            return base.Query()
                .Include(ra => ra.Car);
        }
    }
}
