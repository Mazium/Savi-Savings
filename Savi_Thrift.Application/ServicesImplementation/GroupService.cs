using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using TicketEase.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class GroupService : IGroupService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GroupService> _logger;
		private readonly IMapper _mapper;
		private readonly ICloudinaryServices<Group> _cloudinaryServices;

		public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> logger, IMapper mapper,
			ICloudinaryServices<Group> cloudinaryServices)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_cloudinaryServices = cloudinaryServices;
		}



		public async Task<ApiResponse<GroupResponseDto>> CreateGroupAsync(GroupCreationDto groupCreationDto)
		{
			try
			{
				var isGroupNameUnique = await IsGroupNameUniqueAsync(groupCreationDto.Name);

				if (isGroupNameUnique)
				{
					var groupEntity = _mapper.Map<Group>(groupCreationDto);
					groupEntity.SetAvailableSlots(groupCreationDto.MaxNumberOfParticipants);

					await _unitOfWork.GroupRepository.AddAsync(groupEntity);
					await _unitOfWork.SaveChangesAsync();

					var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

					return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group created successfully", 201);
				}
				else
				{
					return ApiResponse<GroupResponseDto>.Failed("Group name must be unique", 400, null);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a group");

				return ApiResponse<GroupResponseDto>.Failed("Failed to create the group", 500, new List<string> { ex.Message });
			}
		}

		public async Task<bool> IsGroupNameUniqueAsync(string groupName)
		{
			var existingGroup = await _unitOfWork.GroupRepository.FindAsync(g => g.Name == groupName);

			return existingGroup.Count == 0;
		}

		public async Task<ApiResponse<GroupResponseDto>> GetGroupByIdAsync(string groupId)
		{
			try
			{
				var groupEntity = await _unitOfWork.GroupRepository.GetByIdAsync(groupId);

				if (groupEntity == null)
				{
					return ApiResponse<GroupResponseDto>.Failed($"Group with ID {groupId} not found", 404, null);
				}

				var groupResponse = _mapper.Map<GroupResponseDto>(groupEntity);

				return ApiResponse<GroupResponseDto>.Success(groupResponse, "Group retrieved successfully", 200);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error occurred while getting a group by ID ({groupId})");

				return ApiResponse<GroupResponseDto>.Failed("Failed to get the group", 500, new List<string> { ex.Message });
			}
		}

		public async Task<ApiResponse<IEnumerable<GroupResponseDto>>> GetAllGroupsAsync()
		{
			try
			{
				var allGroups = await _unitOfWork.GroupRepository.GetAllAsync();

				var groupResponses = _mapper.Map<IEnumerable<GroupResponseDto>>(allGroups);

				return ApiResponse<IEnumerable<GroupResponseDto>>.Success(groupResponses, "All groups retrieved successfully", 200);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while getting all groups");

				return ApiResponse<IEnumerable<GroupResponseDto>>.Failed("Failed to get all groups", 500, new List<string> { ex.Message });
			}
		}

		public async Task<string> UpdateGroupPhotoByGroupId(string groupId, UpdateGroupPhotoDto model)
		{
			try
			{
				var group = await _unitOfWork.GroupRepository.GetByIdAsync(groupId);

				if (group == null)
					return "Group not found";

				var file = model.PhotoFile;

				if (file == null || file.Length <= 0)
					return "Invalid file size";


				_mapper.Map(model, group);

				var response = await _cloudinaryServices.UploadImage(file);

				if (response == null)
				{
					_logger.LogError($"Failed to upload image for group with ID {groupId}.");
					return null;
				}

				// Update the ImageUrl property with the Cloudinary URL
				group.Avatar = response.Url;

				_unitOfWork.GroupRepository.Update(group);
				await _unitOfWork.SaveChangesAsync();
				return response.Url;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating group photo.");
				throw;
			}
		}

	}
}

