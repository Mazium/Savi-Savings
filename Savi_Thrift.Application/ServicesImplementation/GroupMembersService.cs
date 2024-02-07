using AutoMapper;
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

        public GroupMembersService(IUnitOfWork unitOfWork, ILogger<GroupSavingsService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GroupSavingDetailsResponseDto>> JoinGroupSavingAsync(string groupId, GroupMemberDto userGroupDto)
        {
            try
            {
                var group = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(groupId);

                if (group == null)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("Group not found", 404, null);
                }
                var groupcount = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.GroupSavingsId == groupId);
                if (groupcount.Count == 5)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("This group is filled up already", 400, null);
                }
                var groupMember = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.UserId == userGroupDto.UserId && u.GroupSavingsId == groupId);

                if (groupMember.Count > 0)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("User is already part of the group", 400, null);
                }
                var user = new GroupSavingsMembers
                {
                    UserId = userGroupDto.UserId,
                    Position = (groupMember.Count + 1).ToString(),
                    GroupSavingsId = userGroupDto.GroupSavingsId,
                };
                // Add the user to the group
                await _unitOfWork.GroupMembersRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                if (groupMember.Count + 1 == 5)
                {
                    group.ActualStartDate = DateTime.Now;
                    var frequency = group.Frequency;
                    if (frequency.Equals(SavingFrequency.Daily))
                    {
                        group.ExpectedEndDate = group.ActualStartDate.AddDays(5);
                    }
                    else if (frequency.Equals(SavingFrequency.Weekly))
                    {
                        group.ExpectedEndDate = group.ActualStartDate.AddDays(35);
                    }
                    else
                    {
                        group.ExpectedEndDate = group.ActualStartDate.AddMonths(5);
                    }
                    _unitOfWork.GroupSavingsRepository.Update(group);
                    await _unitOfWork.SaveChangesAsync();
                }

                var groupSavingDetailsDto = new GroupSavingDetailsResponseDto
                {
                    GroupName = group.GroupName,
                    Frequency = group.Frequency,
                    ContributionAmount = group.ContributionAmount,
                    MemberCount = group.MemberCount,
                    ExpectedEndDate = group.ExpectedEndDate,
                };

                return ApiResponse<GroupSavingDetailsResponseDto>.Success(groupSavingDetailsDto, "Explore Group Saving Details", 200);
            }
            catch (Exception ex)
            {
                return ApiResponse<GroupSavingDetailsResponseDto>.Failed("An error occurred while joining the group", 500, new List<string>() { ex.Message });
            }
        }


        /*        public async Task<ApiResponse<GroupSavingDetailsResponseDto>> JoinGroupSavingAsync(string groupId, string userId)
                {
                    try
                    {
                        var group = await _unitOfWork.GroupMembersRepository.GetByIdAsync(groupId);

                        if (group == null)
                        {
                            return ApiResponse<GroupSavingDetailsResponseDto>.Failed("Group not found", 404, null);
                        }

                        var groupCount = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(groupId);
                        if (groupCount.MemberCount >= 5)
                        {
                            return ApiResponse<GroupSavingDetailsResponseDto>.Failed("This group is filled up already", 400, null);
                        }

                        if (groupCount.GroupSavingsMembers.Any(u => u.UserId == userId))
                        {
                            return ApiResponse<GroupSavingDetailsResponseDto>.Failed("User is already part of the group", 400, null);
                        }

                        var user = new GroupSavingsMembers
                        {
                            UserId = userId,
                        };

                        // Add the user to the group
                        groupCount.GroupSavingsMembers.Add(user);

                        // Automatically assign a slot to the user
                        int nextSlot = groupCount.GroupSavingsMembers.Count + 1;

                        // Update the group in the database
                        _unitOfWork.GroupMembersRepository.Update(group);
                        await _unitOfWork.SaveChangesAsync();

                        var groupSavingDetailsDto = new GroupSavingDetailsResponseDto
                        {
                            GroupName = groupCount.GroupName,
                            Frequency = groupCount.Frequency,
                            ContributionAmount = groupCount.ContributionAmount,
                            MemberCount = groupCount.MemberCount,
                            ExpectedEndDate = groupCount.ExpectedEndDate,
                        };

                        return ApiResponse<GroupSavingDetailsResponseDto>.Success(groupSavingDetailsDto, "Explore Group Saving Details", 200);
                    }
                    catch (Exception)
                    {
                        return ApiResponse<GroupSavingDetailsResponseDto>.Failed("An error occurred while joining the group", 500, new List<string>());
                    }
                }
        */
    }
}
