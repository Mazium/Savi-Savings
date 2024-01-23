using Savi_Thrift.Application.DTO.Saving;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface ISavingService
	{
		Task<ApiResponse<GoalResponseDto>> CreateGoal(CreateGoalDto createGoalDto);
		Task<ApiResponse<List<GoalResponseDto>>> ViewGoals();
        Task<ApiResponse<List<Saving>>> GetListOfAllUserGoals(string UserId);
        Task<ApiResponse<SavingsResponseDto>> CreditPersonalSavings(CreditSavingsDto creditDto);
    }
}
