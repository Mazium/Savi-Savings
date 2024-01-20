
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
    }
}
