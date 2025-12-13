using AutoMapper;

namespace ReactifyBlog.Business.MappingProfiles;

public class MapperProfile : Profile
{
  public MapperProfile()
  {
    this.AddUserMappings();
  }
}