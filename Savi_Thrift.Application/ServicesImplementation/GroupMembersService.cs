using AutoMapper;
using Microsoft.Extensions.Logging;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;
using Savi_Thrift.Domain.Entities;
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

        public async Task<ApiResponse<GroupSavingDetailsResponseDto>> JoinGroupSavingAsync(string id, AppUserGroupDto userGroupDto)
        {
            try
            {
                var group = await _unitOfWork.GroupSavingsRepository.GetByIdAsync(id);

                if (group == null)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("Group not found", 404, null);
                }
                var groupcount = await _unitOfWork.GroupMembersRepository.GetByIdAsync(id);
                /*if (group.MemberCount >= 5)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("This group is filled up already", 400, null);
                }
*/
                var groupMember = await _unitOfWork.GroupMembersRepository.FindAsync(u => u.UserId == userGroupDto.UserId);
                if (groupMember == null)
                {
                    return ApiResponse<GroupSavingDetailsResponseDto>.Failed("Group not found", 404, null);
                }
                //if (group.GroupSavingsMembers.Any(u => u.UserId == userGroupDto.UserId))
                /* if (group.GroupSavingsMembers.Any(u => u.UserId == userGroupDto.UserId))
                 {
                     return ApiResponse<GroupSavingDetailsResponseDto>.Failed("User is already part of the group", 400, null);
                 }*/

                var user = new GroupSavingsMembers
                {
                    UserId = userGroupDto.UserId,
                    //  Position = userGroupDto.Position,
                    // GroupSavingsId = userGroupDto.GroupSavingsId,
                };

                // Add the user to the group
                group.GroupSavingsMembers.Add(user);

                // Automatically assign a slot to the user
                int nextSlot = group.GroupSavingsMembers.Count + 1;

                // Update the group in the database
                _unitOfWork.GroupMembersRepository.Update(groupcount);
                await _unitOfWork.SaveChangesAsync();

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
                return ApiResponse<GroupSavingDetailsResponseDto>.Failed("An error occurred while joining the group", 500, new List<string>() { ex.Message});
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
