using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.Interfaces.Services
{
    public interface IGroupMembersService
    {
        Task<ApiResponse<GroupSavingDetailsResponseDto>> JoinGroupSavingAsync(GroupMemberDto userGroupDto);
        Task<ApiResponse<List<GroupMembersDetailsDto>>> GetGroupMembersDetails(string groupId);

	}
}
