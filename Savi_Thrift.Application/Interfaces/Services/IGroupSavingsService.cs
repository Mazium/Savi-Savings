using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IGroupSavingsService
    {
        Task<ApiResponse<GroupResponseDto>> CreateGroupAsync(GroupCreationDto groupCreationDto, string userId);

        Task<ApiResponse<IEnumerable<GroupResponseDto>>> GetAllPublicGroupsAsync();
        Task<ApiResponse<IEnumerable<GroupResponseDto>>> ListOngoingGroupSavingsAccountsAsync();

        Task<ApiResponse<GroupDetailsDto>> GetGroupDetailByIdAsync(string groupId);
        Task<ApiResponse<GroupResponseDto>> ExploreGroupSavingDetailsAsync(string id);

    }
}
