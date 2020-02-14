using System.Threading.Tasks;
using KmLog.Server.Dal.Base;
using KmLog.Server.Dto;
using KmLog.Server.Model;

namespace KmLog.Server.Dal
{
    public interface IUserRepository : IBaseRepository<User, UserDto>
    {
        Task<UserDto> LoadByEmail(string email);
    }
}
