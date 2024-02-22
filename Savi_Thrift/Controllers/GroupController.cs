using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Application.ServicesImplementation;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupSavingsService _groupService;

        public GroupController(IGroupSavingsService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("Add-Groups")]
        public async Task<IActionResult> CreateGroup([FromForm] GroupCreationDto groupCreationDto, string userId)
        {
            if (groupCreationDto == null)
            {
                return BadRequest("Invalid group data");
            }

            var response = await _groupService.CreateGroupAsync(groupCreationDto, userId);

            return Ok(response);
        }

        [HttpGet]
        [Route("ExploreGroups")]
        public async Task<IActionResult> GetAllPublicGroups()
        {
            var response = await _groupService.GetAllPublicGroupsAsync();

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }

		[HttpGet("get-recent-group")]
		public async Task<IActionResult> GetRecentGoup()
		{
			return Ok(await _groupService.GetRecentGroup());
		}


		[HttpGet]
		[Route("GetGroupsByUserId")]
		public async Task<IActionResult> GetGroupsByUserId(string userId)
		{
			var response = await _groupService.GetGroupsByUserId(userId);
            return Ok(response);
		}

		[HttpGet]
        [Route("get-explore-details")]
        public async Task<IActionResult> GetExploreGroupsDetails(string id)
        {
            var response = await _groupService.ExploreGroupSavingDetailsAsync(id);

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }



        [HttpGet]
        [Route("allActiveGroups")]
        public async Task<IActionResult> ListSavingsGroups()
        {
            var response = await _groupService.ListOngoingGroupSavingsAccountsAsync();

            if (response.Succeeded)
            {
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> GetGroupDetailById(string id)
        {
            var response = await _groupService.GetGroupDetailByIdAsync(id);

            if (response != null && response.Succeeded)
            {
                return Ok(response);
            }

            return NotFound(response?.Errors ?? new List<string> { "Error retrieving group detail" });
        }

        [HttpGet("total-savings/{groupId}")]
        public async Task<IActionResult> TotalSavingsGroup(string groupId)
        {
            try
            {
                var response = await _groupService.TotalSavingsGroup(groupId);

                if (response.Succeeded)
                {
                    return Ok(response.Data); // Return the total savings value
                }

                return NotFound(response?.Errors ?? new List<string> { "Error retrieving Total Group Savings" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }



        //        [HttpGet]
        //        [Route("{id}")]
        //        public async Task<IActionResult> GetGroupById(string id)
        //        {
        //            var response = await _groupService.GetGroupByIdAsync(id);

        //            if (response != null && response.Succeeded)
        //            {
        //                return Ok(response);
        //            }

        //            return NotFound(response?.Errors ?? new List<string> { "Error retrieving group" });
        //        }

        //       
        //        [HttpPut]
        //        [Route("updatePhoto/{id}")]
        //        public async Task<IActionResult> UpdateGroupPhotoById(string id, [FromForm] UpdateGroupPhotoDto model)
        //        {
        //            var imageUrl = await _groupService.UpdateGroupPhotoByGroupId(id, model);

        //            if (imageUrl != null)
        //            {
        //                return Ok(new ApiResponse<string>(true, "Group photo updated successfully", 200, imageUrl, null));
        //            }

        //            return StatusCode(500, new ApiResponse<string?>(false, "Failed to update group photo", 500, null, null));
        //        }
    }
}
