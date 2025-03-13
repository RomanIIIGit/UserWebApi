using AutoMapper;
using UserWebApi.Data.Models;
using UserWebApi.Services.Dtos;

namespace UserWebApi.Services.Mappers
{
	public class UserWebApiMappingProfile : Profile
	{
		public UserWebApiMappingProfile()
		{
			CreateMap<User, UserRequestDto>().ReverseMap();
			CreateMap<User, UserResponseDto>().ReverseMap();
		}
	}
}