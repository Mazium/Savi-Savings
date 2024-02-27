using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savi_Thrift.Application.ServicesImplementation
{
	public class GroupMembersService : IGroupMembersService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<GroupSavingsService> _logger;
		private readonly IMapper _mapper;
		private readonly IBackgroundJobClient _backgroundJobClient;

		public GroupMembersService(IUnitOfWork unitOfWork, ILogger<GroupSavingsService> logger, IMapper mapper, IBackgroundJobClient backgroundJobClient)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_backgroundJobClient = backgroundJobClient;
		}



		public async Task<ApiResponse<GroupSavingDetailsResponseDto>> JoinGroupSavingAsync(GroupMemberDto userGroupDto)
		{
			try
			{
				var group = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(userGroupDto.GroupSavingsId);

				if (group == null)
				{
					return ApiResponse<GroupSavingDetailsResponseDto>.Failed("Group not found", 404, null);
				}

				//var kyc = await _unitOfWork.KycRepository.FindAsync(x => x.AppUserId == userGroupDto.UserId);
				//if (kyc.Count == 0)
				//{
				//	return ApiResponse<GroupSavingDetailsResponseDto>.Failed("KYC verification failed. Please complete your KYC before joining group", StatusCodes.Status401Unauthorized, null);
				//}

				var groupcount = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.GroupSavingsId == userGroupDto.GroupSavingsId);
				if (groupcount.Count == 5)
				{
					return ApiResponse<GroupSavingDetailsResponseDto>.Failed("This group is filled up already", 400, null);
				}

				var groupMember = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.UserId == userGroupDto.UserId && u.GroupSavingsId == userGroupDto.GroupSavingsId);

				if (groupMember.Count > 0)
				{
					return ApiResponse<GroupSavingDetailsResponseDto>.Failed("User is already part of the group", 400, null);
				}



				var groupMembers = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.GroupSavingsId == userGroupDto.GroupSavingsId);

				string position = "";
				List<string> list = new List<string>();
				//foreach (var member in groupMembers)
				//{
				//	list.Add(member.Position);
				//}
				list.AddRange(groupMembers.Select(member => member.Position));
				//for (int i = 1; i < 6; i++)
				//{
				//	if (!list.Contains(i.ToString()))
				//	{
				//		position = i.ToString();
				//		break;
				//	}
				//}
				position = Enumerable.Range(1, 5).Select(i => i.ToString()).Except(list).FirstOrDefault();


				var user = new GroupSavingsMembers
				{
					UserId = userGroupDto.UserId,
					Position = position,
					GroupSavingsId = userGroupDto.GroupSavingsId,
					HasCollected = false
				};
				// Add the user to the group
				await _unitOfWork.GroupMembersRepository.AddAsync(user);
				await _unitOfWork.SaveChangesAsync();

				var today = DateTime.Now;

				if (groupMembers.Count + 1 == 5)
				{
					group.GroupStatus = GroupStatus.Ongoing;
					var currentRuntime = group.RunTime;

					DateTime scheduledTime;

					if (today.TimeOfDay > currentRuntime.TimeOfDay)
					{
						DateTime tomorrow = today.AddDays(1);
						scheduledTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, currentRuntime.Hour, currentRuntime.Minute, 0);
					}
					else
					{
						scheduledTime = new DateTime(today.Year, today.Month, today.Day, currentRuntime.Hour, currentRuntime.Minute, 0);
					}
					group.ActualStartDate = scheduledTime;
					group.RunTime = scheduledTime;
					_backgroundJobClient.Schedule<IRecurringGroupJobs>((job) => job.FundNow(group.Id), scheduledTime);					
					_unitOfWork.GroupSavingsRepository.Update(group);
					await _unitOfWork.SaveChangesAsync();
				}

				var groupSavingDetailsDto = new GroupSavingDetailsResponseDto
				{
					GroupName = group.GroupName,
					Frequency = group.Frequency,
					ContributionAmount = group.ContributionAmount,
					MemberCount = group.MembersMaximumCount,
					ExpectedEndDate = group.ExpectedEndDate,
				};

				return ApiResponse<GroupSavingDetailsResponseDto>.Success(groupSavingDetailsDto, "You have successfully joined " + group.GroupName, StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				return ApiResponse<GroupSavingDetailsResponseDto>.Failed("An error occurred while joining the group", 500, new List<string>() { ex.Message });
			}
		}


		public async Task<ApiResponse<List<GroupMembersDetailsDto>>> GetGroupMembersDetails(string groupId)
		{
			try
			{
				var groupMembers = await _unitOfWork.GroupMembersRepository.FindAsync(x => x.GroupSavingsId == groupId);
				if (groupMembers.Count == 0)
				{
					return ApiResponse<List<GroupMembersDetailsDto>>.Failed("Group not found", StatusCodes.Status404NotFound, null);
				}
				List<GroupMembersDetailsDto> groupMembersDetailsDtos = new();

				foreach (var member in groupMembers)
				{

					var user = await _unitOfWork.UserRepository.GetByIdAsync(member.UserId);

					groupMembersDetailsDtos.Add(new GroupMembersDetailsDto
					{
						Id = member.UserId,
						Name = user.FirstName + " " + user.LastName,
						Position = member.Position,
						Performance = 0
					});
				}
				return ApiResponse<List<GroupMembersDetailsDto>>.Success(groupMembersDetailsDtos, "Group members retrieved successfully ", StatusCodes.Status200OK);

			}
			catch (Exception e)
			{
				return ApiResponse<List<GroupMembersDetailsDto>>.Failed("Group not found", StatusCodes.Status500InternalServerError, new List<string> { e.Message });
			}


		}

	}
}
