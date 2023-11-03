using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;

namespace Explorer.Stakeholders.Core.Mappers;

public class StakeholderProfile : Profile
{
	public StakeholderProfile()
    {
        CreateMap<ClubRequestDto, ClubRequest>().ReverseMap();
        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<ClubInvitationDto, ClubInvitation>().ReverseMap();
        CreateMap<ApplicationGradeDto, ApplicationGrade>().ReverseMap();
        CreateMap<PersonDto, Person>().ReverseMap();
        CreateMap<ClubDto, Club>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<MessageDto,  Message>().ReverseMap();
        CreateMap<UserProfileDto, User>().ReverseMap();
    }
}