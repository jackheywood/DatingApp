using AutoMapper;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Domain.Entities;
using DatingApp.Backend.Domain.Extensions;

namespace DatingApp.Backend.Application.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

        CreateMap<MemberUpdateDto, AppUser>();

        CreateMap<Photo, PhotoDto>();
    }
}
