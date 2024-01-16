

using AutoMapper;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using TicketEase.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public UserService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<RegisterResponseDto>>> GetUsers()
		{
			var users = await _unitOfWork.UserRepository.GetAllAsync();
			List<RegisterResponseDto> result = new();
			foreach (var user in users)
			{
				var reponseDto = _mapper.Map<RegisterResponseDto>(user);
				result.Add(reponseDto);
			}
			return new ApiResponse<List<RegisterResponseDto>>(result, "Users retrieved successfully");
		}

	}
}
