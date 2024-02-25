using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Enums;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class GroupSavingsService : IGroupSavingsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GroupSavingsService> _logger;
		private readonly IMapper _mapper;
		private readonly ICloudinaryServices<GroupSavings> _cloudinaryServices;

		public GroupSavingsService(IUnitOfWork unitOfWork, ILogger<GroupSavingsService> logger, IMapper mapper,
			ICloudinaryServices<GroupSavings> cloudinaryServices)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_cloudinaryServices = cloudinaryServices;
		}


		public async Task<ApiResponse<GroupResponseDto>> CreateGroupAsync(GroupCreationDto groupCreationDto, string userId)
		{
			try
			{
				var groups = await _unitOfWork.GroupSavingsRepository.FindAsync(x => x.GroupName == groupCreationDto.GroupName);

				if (groups.Any())
				{
					return ApiResponse<GroupResponseDto>.Failed("Group name already exist", StatusCodes.Status403Forbidden, null);
				}

				var groupEntity = _mapper.Map<GroupSavings>(groupCreationDto);

				if (groupCreationDto.SafePortraitImageURL != null)
				{
					var portrait = await _cloudinaryServices.UploadImage(groupCreationDto.SafePortraitImageURL);
					if (portrait == null)
					{
						return ApiResponse<GroupResponseDto>.Failed("Failed to upload portrait images to cloudinary.",
							StatusCodes.Status500InternalServerError, new List<string>());
					}
					groupEntity.SafePortraitImageURL = portrait.Url;
				}

				if (groupCreationDto.SafeLandScapeImageURL != null)
				{
					var portrait = await _cloudinaryServices.UploadImage(groupCreationDto.SafeLandScapeImageURL);
					if (portrait == null)
					{
						return ApiResponse<GroupResponseDto>.Failed("Failed to upload SafeLandScapeImage to cloudinary.",
							StatusCodes.Status500InternalServerError, new List<string>());
					}
					groupEntity.SafeLandScapeImageURL = portrait.Url;
				}


				await _unitOfWork.GroupSavingsRepository.AddAsync(groupEntity);
				await _unitOfWork.SaveChangesAsync();


				var user = new GroupSavingsMembers
				{
					UserId = userId,
					Position = "1",
					GroupSavingsId = groupEntity.Id,
					IsGroupOwner = true,
				};
				// Add the user to the group
				await _unitOfWork.GroupMembersRepository.AddAsync(user);
				await _unitOfWork.SaveChangesAsync();

				var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);
				return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group created successfully", 201);



			}
			catch (Exception ex)
			{

				_logger.LogError(ex, "Error occurred while creating a group");

				return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.InnerException.ToString() });

			}
		}

		public async Task<ApiResponse<IEnumerable<GroupResponseDto>>> GetAllPublicGroupsAsync()
		{
			try
			{
				var allGroups = await _unitOfWork.GroupSavingsRepository.FindAsync(x => x.IsOpen == true && x.IsDeleted == false && x.GroupStatus==GroupStatus.Waiting);

				foreach (var groupEntity in allGroups)
				{
					groupEntity.GroupSavingsMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.GroupSavingsId == groupEntity.Id);
				}

				var groupResponses = _mapper.Map<IEnumerable<GroupResponseDto>>(allGroups);

				return ApiResponse<IEnumerable<GroupResponseDto>>.Success(groupResponses, "All groups retrieved successfully", 200);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting all groups");

				return ApiResponse<IEnumerable<GroupResponseDto>>.Failed("Failed to get all groups", 500, new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<IEnumerable<GroupResponseDto>>> GetGroupsByUserId(string userId)
		{



			try
			{
				// Retrieve all ongoing group savings accounts and filter them where GroupStatus is 'Ongoing'
				var ongoingGroups = await _unitOfWork.GroupSavingsRepository.FindAsync(g => g.GroupStatus == GroupStatus.Ongoing);

				List <GroupSavings> userActiveGroups = new();

				foreach (var ongoingGroup in ongoingGroups)
				{
					var myGroupMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.UserId == userId && x.IsDeleted == false && x.GroupSavingsId==ongoingGroup.Id);
					if(myGroupMembers.Count() > 0)
					{
						userActiveGroups.Add(ongoingGroup);
					}
				}
				var groupResponses = _mapper.Map<IEnumerable<GroupResponseDto>>(userActiveGroups);

				return ApiResponse<IEnumerable<GroupResponseDto>>.Success(
					groupResponses,
					"Ongoing group saving accounts retrieved successfully",
					200
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting ongoing group savings accounts");

				return ApiResponse<IEnumerable<GroupResponseDto>>.Failed(
					"Failed to get ongoing group saving accounts",
					500,
					new List<string> { ex.Message }
				);
			}
		}


		public async Task<ApiResponse<IEnumerable<GroupResponseDto>>> ListOngoingGroupSavingsAccountsAsync()
		{
			try
			{
				// Retrieve all ongoing group savings accounts and filter them where GroupStatus is 'Ongoing'
				var allGroups = await _unitOfWork.GroupSavingsRepository.GetAllAsync();

				var ongoingGroups = allGroups.Where(g => g.GroupStatus == GroupStatus.Ongoing);

				var groupResponses = _mapper.Map<IEnumerable<GroupResponseDto>>(ongoingGroups);

				return ApiResponse<IEnumerable<GroupResponseDto>>.Success(
					groupResponses,
					"Ongoing group saving accounts retrieved successfully",
					200
				);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting ongoing group savings accounts");

				return ApiResponse<IEnumerable<GroupResponseDto>>.Failed(
					"Failed to get ongoing group saving accounts",
					500,
					new List<string> { ex.Message }
				);
			}
		}


		public async Task<ApiResponse<GroupResponseDto>> GetGroupDetailByIdAsync(string groupId)
		{
			try
			{
				var groupEntity = await _unitOfWork.GroupSavingsRepository
					.FindSingleAsync(g => g.Id == groupId && g.GroupStatus == GroupStatus.Ongoing);

				if (groupEntity == null)
				{
					return ApiResponse<GroupResponseDto>.Failed(
						$"Group with ID {groupId} not found or is inactive/open",
						404,
						null
					);
				}

				var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

				// Return a successful ApiResponse with the mapped GroupDetailsDto
				return ApiResponse<GroupResponseDto>.Success(
					groupResponse,
					"Group retrieved successfully",
					200
				);
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during the process
				_logger.LogError(ex, $"Error occurred while getting a group by ID ({groupId})");

				// Return a failed ApiResponse with details of the error
				return ApiResponse<GroupResponseDto>.Failed(
					"Failed to get the group",
					500,
					new List<string> { ex.Message }
				);
			}
		}

        public async Task<ApiResponse<List<GroupResponseDto>>> GetRecentGroup()
        {

			var today = DateTime.Today;
			var newGroupSavings = await _unitOfWork.GroupSavingsRepository.FindAsync(u => u.CreatedAt >=today && u.CreatedAt<today.AddDays(1));
	
            if(newGroupSavings.Count == 0)
               return ApiResponse<List<GroupResponseDto>>.Failed("No recent groups found", StatusCodes.Status404NotFound, null);

            var groupResponses = _mapper.Map<List<GroupResponseDto>>(newGroupSavings);

            return ApiResponse<List<GroupResponseDto>>.Success(groupResponses, "Recent groups successfully retrieved", StatusCodes.Status200OK);
        }

        public async Task<ApiResponse<GroupResponseDto>> ExploreGroupSavingDetailsAsync(string id)
        {
            try
            {
                var groupDetails = await _unitOfWork.GroupSavingsRepository.FindAsync(u => u.Id == id && u.IsDeleted == false);

				if (groupDetails.Count == 0)
				{
					return ApiResponse<GroupResponseDto>.Failed($"Group not found", 404, null);
				}
				var groupDetail = groupDetails.First();
				groupDetail.GroupSavingsMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.GroupSavingsId == groupDetail.Id);

				var groupResponses = _mapper.Map<GroupResponseDto>(groupDetail);

				return ApiResponse<GroupResponseDto>.Success(groupResponses, $"Explore Group Saving Details", 200);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a group");
				return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.InnerException.ToString() });

			}
		}

	

	}
}

