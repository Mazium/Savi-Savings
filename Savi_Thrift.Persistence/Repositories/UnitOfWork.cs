using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SaviDbContext _context;
        public UnitOfWork(SaviDbContext context)
        {
            _context = context;
            KycRepository = new KycRepository(_context);
        }

        public IKycRepository KycRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
