using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        private readonly IGroupMembersService _groupMembersService;

        public GroupMembersController(IGroupMembersService groupMembersService)
        {
           _groupMembersService = groupMembersService;
        }
		

		[HttpPost]
        [Route("join-group")]
        public async Task<IActionResult> JoinGroupsSavings(GroupMemberDto userGroupDto)
        {
            var response = await _groupMembersService.JoinGroupSavingAsync(userGroupDto);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }

		[HttpGet]
		[Route("getGroupMembers")]
		public async Task<IActionResult> GetGroupMembersDetails(string groupId)
		{
				return Ok(await _groupMembersService.GetGroupMembersDetails(groupId));
		}
	}
}
