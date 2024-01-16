using Savi_Thrift.Application.DTO.Saving;
using TicketEase.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface ISavingService
	{
		Task<ApiResponse<GoalResponseDto>> CreateGoal(CreateGoalDto createGoalDto);
		Task<ApiResponse<List<GoalResponseDto>>> ViewGoals();
	}
}
