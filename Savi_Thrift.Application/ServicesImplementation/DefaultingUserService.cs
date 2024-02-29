using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.DefaultUser;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain;



namespace Savi_Thrift.Application.ServicesImplementation
{
    public class DefaultingUserService : IDefaultingUserService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DefaultingUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<DefaultUserDto>>> GetDefaultingUsers(string groupSavingsId)
        {
			var defaultingUsersDtos = new List<DefaultUserDto>();
			var defaultingUsers = await _unitOfWork.DefaultingUserRepository.FindAsync(x => x.GroupSavingId == groupSavingsId);
            if(defaultingUsers.Count ==0)
            {
				return ApiResponse<List<DefaultUserDto>>.Success(defaultingUsersDtos, "No defaulting user found", StatusCodes.Status200OK);
			}

           

            foreach (var defaultingUser in defaultingUsers)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(defaultingUser.AppUserId); 
                
                if(user != null)
                {
                    var defaultinUserDto = new DefaultUserDto()
                    {
                       Name = user.FirstName + " " + user.LastName,
                       Email = user.Email,
                       ImageUrl = user.ImageUrl,
                       LastLoginTime = user.LastLogin,
						DefaultDateTime= defaultingUser.ActualDebitDate
					};
                    defaultingUsersDtos.Add(defaultinUserDto);
				}
            }

            var defaultUser = _mapper.Map<List<DefaultUserDto>>(defaultingUsersDtos);
			return ApiResponse<List<DefaultUserDto>>.Success(defaultUser, "Defaulting Users Retrieved Successfully", StatusCodes.Status200OK);
		}
		public async Task<ApiResponse<List<DefaultUserDto>>> GetAllDefaultingUsers()
		{
			var defaultingUsersDtos = new List<DefaultUserDto>();

			var groups = await _unitOfWork.GroupSavingsRepository.FindAsync(x=>x.IsDeleted == false);
			foreach (var group in groups)
			{
				var defaultingUsers = await _unitOfWork.DefaultingUserRepository.FindAsync(x => x.GroupSavingId == group.Id);
				if (defaultingUsers.Count >0)
				{
					foreach (var defaultingUser in defaultingUsers)
					{
						var user = await _unitOfWork.UserRepository.GetByIdAsync(defaultingUser.AppUserId);

						if (user != null)
						{
							var defaultinUserDto = new DefaultUserDto()
							{
								Name = user.FirstName + " " + user.LastName,
								Email = user.Email,
								ImageUrl = user.ImageUrl,
								LastLoginTime = user.LastLogin,
								DefaultDateTime = defaultingUser.ActualDebitDate
							};
							defaultingUsersDtos.Add(defaultinUserDto);
						}
					}
				}

			}

			var defaultUser = _mapper.Map<List<DefaultUserDto>>(defaultingUsersDtos);
			return ApiResponse<List<DefaultUserDto>>.Success(defaultUser, "Defaulting Users Retrieved Successfully", StatusCodes.Status200OK);
		}
        public async Task<ApiResponse<List<DefaultUserDto>>> GetTodayDefaultingUsers()
        {
            DateTime today = DateTime.Today;
            var newUsers = await _unitOfWork.DefaultingUserRepository.FindAsync(u => u.IsDeleted == false && u.CreatedAt.Day == today.Day && u.CreatedAt.Month == today.Month && u.CreatedAt.Year==today.Year);
            var users = _mapper.Map<List<DefaultUserDto>>(newUsers);
            return ApiResponse<List<DefaultUserDto>>.Success(users, "List of New Defaulting Users Retrieved Successfully", StatusCodes.Status200OK);
        }
    }

}
