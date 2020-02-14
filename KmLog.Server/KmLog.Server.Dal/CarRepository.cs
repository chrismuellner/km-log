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
    public class CarRepository : BaseRepository<Car, CarDto>, ICarRepository
    {
        public CarRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }

        public async Task<IEnumerable<CarDto>> LoadByUser(Guid userId)
        {
            var cars = await Query()
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Mapper.Map<IEnumerable<CarDto>>(cars);
        }

        protected override IQueryable<Car> Query()
        {
            return base.Query()
                .Include(c => c.RefuelActions);
        }
    }
}
