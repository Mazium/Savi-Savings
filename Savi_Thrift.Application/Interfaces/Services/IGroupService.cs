using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IGroupService
    {
        Task<ApiResponse<GroupResponseDto>> CreateGroupAsync(GroupCreationDto groupCreationDto);
        Task<bool> IsGroupNameUniqueAsync(string groupName);
        Task<ApiResponse<GroupResponseDto>> GetGroupByIdAsync(string groupId);
        Task<ApiResponse<IEnumerable<GroupResponseDto>>> GetAllGroupsAsync();
        Task<string> UpdateGroupPhotoByGroupId(string groupId, UpdateGroupPhotoDto model);
    }
}
