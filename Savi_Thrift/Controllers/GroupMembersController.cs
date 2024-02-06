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
        public async Task<IActionResult> JoinGroupsSavings(string id, AppUserGroupDto userGroupDto)
        {
            var response = await _groupMembersService.JoinGroupSavingAsync(id, userGroupDto);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }
    }
}
