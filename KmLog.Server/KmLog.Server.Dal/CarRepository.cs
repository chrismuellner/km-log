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
    public class CarRepository : IdentifiableBaseRepository<Car, CarDto>, ICarRepository
    {
        public CarRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }

        public async Task<CarDto> LoadByLicensePlate(string licensePlate)
        {
            var car = await Query()
                .FirstOrDefaultAsync(c => c.LicensePlate == licensePlate);

            return Mapper.Map<CarDto>(car);
        }

        public async Task<IEnumerable<CarInfoDto>> LoadByUser(Guid userId)
        {
            var cars = await Query()
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<CarInfoDto>>(cars);
        }

        public async Task<IEnumerable<CarInfoDto>> LoadByGroup(Guid groupId)
        {
            var cars = await Query()
                .Where(c => c.User.GroupId == groupId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<CarInfoDto>>(cars);
        }

        public async Task<CarStatisticDto> LoadStatisticByLicensePlate(string licensePlate)
        {
            var statistic = await Context.RefuelEntries
                .Where(r => r.Car.LicensePlate == licensePlate)
                .GroupBy(r => r.Car.LicensePlate, (k, g) => new CarStatisticDto
                {
                    AvgCost = g.Average(r => r.Cost),
                    AvgDistance = g.Average(r => r.Distance),
                    TotalCost = g.Sum(r => r.Cost),
                    TotalDistance = g.Max(r => r.TotalDistance)
                })
                .FirstOrDefaultAsync();

            return statistic;
        }

        protected override IQueryable<Car> Query()
        {
            return base.Query()
                .Include(c => c.RefuelEntries);
        }
    }
}
