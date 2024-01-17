using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Persistence.Context;
using System.Linq.Expressions;

namespace Savi_Thrift.Persistence.Repositories
{
    public class KycRepository : GenericRepository<KYC>, IKycRepository
    {
        public KycRepository(SaviDbContext context) : base(context) { }

        public async Task AddKycAsync(KYC kyc) => AddAsync(kyc);

        public async Task DeleteKycAsync(KYC kyc) => DeleteAsync(kyc);
        public async Task<List<KYC>> FindKycs(Expression<Func<KYC, bool>> expression) => await FindAsync(expression);

        public async Task<KYC> GetKycByIdAsync(string id) => await GetByIdAsync(id);
        public async Task<List<KYC>> GetAllKycs() => await GetAllAsync();

        public void UpdateKyc(KYC kyc) => Update(kyc);
    }
}
