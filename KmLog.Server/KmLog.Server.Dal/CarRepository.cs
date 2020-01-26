using System.Linq;
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

        protected override IQueryable<Car> Query()
        {
            return base.Query()
                .Include(c => c.Journeys);
        }
    }
}
