using Microsoft.AspNetCore.Mvc;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Services;
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
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationDto groupCreationDto)
        {
            if (groupCreationDto == null)
            {
                return BadRequest("Invalid group data");
            }

            var response = await _groupService.CreateGroupAsync(groupCreationDto);

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
