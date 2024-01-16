using AutoMapper;
using Savi_Thrift.Application.DTO.AppUser;
using Savi_Thrift.Application.DTO.Group;
using Savi_Thrift.Application.DTO.Saving;
using Savi_Thrift.Application.DTO.Wallet;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
			CreateMap<CreateWalletDto, Wallet>().ReverseMap();
			CreateMap<GroupResponseDto, Group>().ReverseMap();
			CreateMap<GroupCreationDto, Group>().ReverseMap();
			CreateMap<Wallet, WalletResponseDto>().ReverseMap();
			CreateMap<CreateGoalDto, Saving>().ReverseMap();
			CreateMap<GoalResponseDto, Saving>().ReverseMap();
			CreateMap<RegisterResponseDto, AppUser>().ReverseMap();
			CreateMap<FundWalletDto, WalletFunding>().ReverseMap();
		}  
    }
}
