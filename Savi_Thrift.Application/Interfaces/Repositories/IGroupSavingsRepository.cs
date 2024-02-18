using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application.Interfaces.Repositories
{
    public interface IGroupSavingsRepository: IGenericRepository<GroupSavings>
    {
        Task<List<GroupSavings>> GetNewGroupSavings();
    }
}
