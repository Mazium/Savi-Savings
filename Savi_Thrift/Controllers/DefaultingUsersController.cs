using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DefaultingUsersController : ControllerBase
	{
		private readonly IDefaultingUserService _defaultingUserService;

        public DefaultingUsersController(IDefaultingUserService defaultingUserService)
        {
            _defaultingUserService = defaultingUserService;
        }

		[HttpGet("GetDefaultingUsersByGroupId")]

		public async Task<IActionResult> GetAllDefaultingUsersByGroupId(string groupSavingsId)
		{
			return Ok(await _defaultingUserService.GetDefaultingUsers(groupSavingsId));
		}
    }
}
