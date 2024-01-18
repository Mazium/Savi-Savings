using Savi_Thrift.Domain.Entities;
using System.Linq.Expressions;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IKycRepository : IGenericRepository<KYC>
    {
        Task<KYC> GetKycByIdAsync(string id);
        Task<List<KYC>> GetAllKycs();
        Task AddKycAsync(KYC kyc);
        Task DeleteKycAsync(KYC kyc);
        void UpdateKyc(KYC kyc);
        Task<List<KYC>> FindKycs(Expression<Func<KYC, bool>> expression);
    }
}
