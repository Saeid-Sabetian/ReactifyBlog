using AutoMapper;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Business.MappingProfiles;

public static class UserMappingProfile
{
  public static void AddUserMappings(this Profile profile)
  {
    profile.CreateMap<RegisterRequest, UserDBO>()
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLower()))
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Trim().ToLower()));
  }
}
