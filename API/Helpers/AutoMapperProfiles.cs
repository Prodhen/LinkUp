using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
        .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
      .ForMember(
          dest => dest.PhotoUrl,
          opt => opt.MapFrom(src =>
              src.Photos.FirstOrDefault(p => p.IsMain)!.Url
          )
      );
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdatesDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
    }

}
