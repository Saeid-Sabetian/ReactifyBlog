using AutoMapper;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Business.MappingProfiles;

public class UserProfile : Profile
{
  public UserProfile()
  {
    CreateMap<RegisterRequest, UserDBO>()
        .ForMember(dest => dest.Email, otp => otp.MapFrom(src => src.Email.Trim().ToLower()))
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Trim().ToLower()));
  }
}
