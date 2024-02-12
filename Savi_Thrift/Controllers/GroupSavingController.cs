using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.DTO.UserTransaction;
using Savi_Thrift.Application.Interfaces.Services;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupSavingController : ControllerBase
    {
        private readonly IGroupSavingsService _groupSavingsService;

        public GroupSavingController(IGroupSavingsService groupSavingsService)
        {
            _groupSavingsService = groupSavingsService;
        }

        [HttpGet("get-recent-group")]
        public async Task<IActionResult> GetRecentGoup()
        {
            var groupEntity = await _groupSavingsService.GetRecentGroup();
            if (groupEntity.Data == null)
                return NotFound();
            return Ok(groupEntity);
        }
    }
}
