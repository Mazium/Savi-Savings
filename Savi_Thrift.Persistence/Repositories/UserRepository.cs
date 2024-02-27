

using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Application.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;

namespace Savi_Thrift.Persistence.Repositories
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        private readonly SaviDbContext _saviDbContext;

        public UserRepository(SaviDbContext saviDbContext) : base(saviDbContext)
        {
            _saviDbContext = saviDbContext;
        }

       
    }
}
