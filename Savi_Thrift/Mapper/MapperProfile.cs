using AutoMapper;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<KycRequestDto, KYC>().ReverseMap();
            CreateMap<KYC, KycResponseDto>().ReverseMap();
        }
    }
}
