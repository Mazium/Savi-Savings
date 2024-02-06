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
                    return ApiResponse<GroupResponseDto>.Failed("Group name must be unique", 400, null);
                }

                else
                {
                    var groupEntity = _mapper.Map<GroupSavings>(groupCreationDto);
                    await _unitOfWork.GroupSavingsRepository.AddAsync(groupEntity);
                    await _unitOfWork.SaveChangesAsync();


                    var user = new GroupSavingsMembers
                    {
                        UserId = userId,
                        Position = "1",
                        GroupSavingsId = groupEntity.Id,
                    };
                    // Add the user to the group
                    await _unitOfWork.GroupMembersRepository.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();                  




                    var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

                    return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group created successfully", 201);


                }
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
                var allGroups = await _unitOfWork.GroupSavingsRepository.FindAsync(x => x.IsOpen == true && x.IsDeleted == false);


                var groupResponses = _mapper.Map<IEnumerable<GroupResponseDto>>(allGroups);

                return ApiResponse<IEnumerable<GroupResponseDto>>.Success(groupResponses, "All groups retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all groups");

                return ApiResponse<IEnumerable<GroupResponseDto>>.Failed("Failed to get all groups", 500, new List<string> { ex.Message });
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


        public async Task<ApiResponse<GroupDetailsDto>> GetGroupDetailByIdAsync(string groupId)
        {
            try
            {
                // Retrieve the group entity by ID using the repository
                var groupEntity = await _unitOfWork.GroupSavingsRepository
                    .FindSingleAsync(g => g.Id == groupId && g.GroupStatus == GroupStatus.Ongoing);

                // Check if the group entity is null, indicating that the group was not found
                if (groupEntity == null)
                {
                    return ApiResponse<GroupDetailsDto>.Failed(
                        $"Group with ID {groupId} not found or is inactive/open",
                        404,
                        null
                    );
                }

                // Map the group entity to GroupDetailsDto using AutoMapper
                var groupResponse = _mapper.Map<GroupDetailsDto>(groupEntity);

                // Return a successful ApiResponse with the mapped GroupDetailsDto
                return ApiResponse<GroupDetailsDto>.Success(
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
                return ApiResponse<GroupDetailsDto>.Failed(
                    "Failed to get the group",
                    500,
                    new List<string> { ex.Message }
                );
            }
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
                var groupResponses = _mapper.Map<GroupResponseDto>(groupDetail);

                return ApiResponse<GroupResponseDto>.Success(groupResponses, $"Explore Group Saving Details", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a group");
                return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.InnerException.ToString() });

            }
        }

        //		public async Task<ApiResponse<GroupResponseDto>> CreateGroupAsync(GroupCreationDto groupCreationDto)
        //		{
        //			try
        //			{
        //				var isGroupNameUnique = await IsGroupNameUniqueAsync(groupCreationDto.Name);

        //				if (isGroupNameUnique)
        //				{
        //					var groupEntity = _mapper.Map<Group>(groupCreationDto);
        //					groupEntity.SetAvailableSlots(groupCreationDto.MaxNumberOfParticipants);
        //					groupEntity.IsActive = true;

        //					await _unitOfWork.GroupRepository.AddAsync(groupEntity);
        //					await _unitOfWork.SaveChangesAsync();

        //					var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

        //					return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group created successfully", 201);
        //				}
        //				else
        //				{
        //					return ApiResponse<GroupResponseDto>.Failed("Group name must be unique", 400, null);
        //				}
        //			}
        //			catch (Exception ex)
        //			{
        //				_logger.LogError(ex, "Error occurred while creating a group");

        //				return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.InnerException.ToString() });
        //			}
        //		}

        //        public async Task<ApiResponse<GroupResponseDto>> CreateNewGroupSavingsAsync(GroupCreationDto groupCreationDto)
        //        {
        //            try
        //            {
        //                var isGroupNameUnique = await IsGroupNameUniqueAsync(groupCreationDto.Name);

        //                if (isGroupNameUnique)
        //                {
        //                    var groupEntity = _mapper.Map<Group>(groupCreationDto);
        //                    groupEntity.SetAvailableSlots(groupCreationDto.MaxNumberOfParticipants);
        //                    groupEntity.IsActive = true;

        //                    // Add logic to assign participants and handle the bulk contribution cycle
        //                    var participants = new List<string> { groupCreationDto.CreatorName }; // Assuming CreatorName is a property in GroupCreationDto

        //                    // Assign remaining slots to participants
        //                    for (int i = 1; i < groupCreationDto.MaxNumberOfParticipants; i++)
        //                    {
        //                        participants.Add($"Participant{i + 1}");
        //                    }

        //                    groupEntity.Participants = participants; // Add Participants property to your Group entity

        //                    await _unitOfWork.GroupRepository.AddAsync(groupEntity);
        //                    await _unitOfWork.SaveChangesAsync();

        //                    var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

        //                    return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group created successfully", 201);
        //                }
        //                else
        //                {
        //                    return ApiResponse<GroupResponseDto>.Failed("Group name must be unique", 400, null);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error occurred while creating a group");

        //                return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.InnerException.ToString() });
        //            }
        //        }


        //        public async Task<bool> IsGroupNameUniqueAsync(string groupName)
        //		{
        //			var existingGroup = await _unitOfWork.GroupRepository.FindAsync(g => g.Name == groupName);

        //			return existingGroup.Count == 0;
        //		}

        //		public async Task<ApiResponse<GroupResponseDto>> GetGroupByIdAsync(string groupId)
        //		{
        //			try
        //			{
        //				var groupEntity = await _unitOfWork.GroupRepository.GetByIdAsync(groupId);

        //				if (groupEntity == null)
        //				{
        //					return ApiResponse<GroupResponseDto>.Failed($"Group with ID {groupId} not found", 404, null);
        //				}

        //				var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

        //				return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group retrieved successfully", 200);
        //			}
        //			catch (Exception ex)
        //			{
        //				_logger.LogError(ex, $"Error occurred while getting a group by ID ({groupId})");

        //				return ApiResponse<GroupResponseDto>.Failed("Failed to get the group", 500, new List<string> { ex.Message });
        //			}
        //		}

        //		

        //		public async Task<string> UpdateGroupPhotoByGroupId(string groupId, UpdateGroupPhotoDto model)
        //		{
        //			try
        //			{
        //				var group = await _unitOfWork.GroupRepository.GetByIdAsync(groupId);

        //				if (group == null)
        //					return "Group not found";

        //				var file = model.PhotoFile;

        //				if (file == null || file.Length <= 0)
        //					return "Invalid file size";


        //				_mapper.Map(model, group);

        //				var response = await _cloudinaryServices.UploadImage(file);

        //				if (response == null)
        //				{
        //					_logger.LogError($"Failed to upload image for group with ID {groupId}.");
        //					return null;
        //				}

        //				// Update the ImageUrl property with the Cloudinary URL
        //				group.Avatar = response.Url;

        //				_unitOfWork.GroupRepository.Update(group);
        //				await _unitOfWork.SaveChangesAsync();
        //				return response.Url;
        //			}
        //			catch (Exception ex)
        //			{
        //				_logger.LogError(ex, "An error occurred while updating group photo.");
        //				throw;
        //			}
        //		}


    }
}

