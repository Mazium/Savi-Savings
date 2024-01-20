

using Savi_Thrift.Application.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
		public UserRepository(SaviDbContext context) : base(context) { }
	}
}
