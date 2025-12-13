using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.Contracts.Wrappers;
using ReactifyBlog.Business.MappingProfiles;
using ReactifyBlog.Business.Services;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Business.Tests.Services;

public class AuthenticationServiceTests
{
  private class SutFactory
  {
    public Mock<UserManager<UserDBO>> UserManager = new Mock<UserManager<UserDBO>>();
    public Mock<SignInManager<UserDBO>> SignInManager = new Mock<SignInManager<UserDBO>>();
    public Mock<IHttpContextAccessorWrapper> HttpContextAccessorWrapper = new Mock<IHttpContextAccessorWrapper>();
    public Mock<IEmailService> EmailService = new Mock<IEmailService>();
    public Mock<ReactifyBlogDbContext> ReactifyBlogDbContext = new Mock<ReactifyBlogDbContext>();
    public IMapper Mapper { get; }

    public SutFactory()
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MapperProfile>();
      });

      Mapper = config.CreateMapper();
    }

    public AuthenticationService CreateSut()
    {
      return new AuthenticationService(
          UserManager.Object,
          SignInManager.Object,
          HttpContextAccessorWrapper.Object,
          EmailService.Object,
          Mapper,
          ReactifyBlogDbContext.Object
      );
    }
  }
}
