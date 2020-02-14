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
    public class UserRepository : BaseRepository<User, UserDto>, IUserRepository
    {
        public UserRepository(KmLogContext context, IMapper mapper) : base(context, mapper)
        { }

        public async Task<UserDto> LoadByEmail(string email)
        {
            var user = await Query()
                .FirstOrDefaultAsync(u => u.Email == email);

            return Mapper.Map<UserDto>(user);
        }
    }
}
