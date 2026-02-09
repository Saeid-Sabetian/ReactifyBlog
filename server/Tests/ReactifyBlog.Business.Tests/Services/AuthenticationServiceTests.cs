using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using ReactifyBlog.Business.Constants.ErrorConstants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.Contracts.Wrappers;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.Exceptions;
using ReactifyBlog.Business.MappingProfiles;
using ReactifyBlog.Business.Services;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;
using ReactifyBlog.UnitTestToolkit;

namespace ReactifyBlog.Business.Tests.Services;

public class AuthenticationServiceTests
{
  [Fact]
  public async Task RegisterUserAsync_WhenCreateUserFails_ShouldThrowException()
  {
    // Arrange
    using var factory = new SutFactory();

    // Mock کردن UserManager برای شکست CreateAsync
    factory.UserManager
        .Setup(x => x.CreateAsync(It.IsAny<UserDBO>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Failed(
            new IdentityError { Code = IdentityErrorCodeConstants.DuplicateEmail }));

    var sut = await factory.CreateSut();

    var request = new RegisterRequest
    {
      Email = "test@test.com",
      Password = "Password123!"
    };

    // Act
    Func<Task> act = async () => await sut.RegisterUserAsync(request);

    // Assert
    await act.Should()
        .ThrowAsync<ReactifyBlogException>()
        .Where(e => e.ErrorCode == AuthServiceErrorConstants.RegisterDuplicateEmailErrorCode);

    // Optional: بررسی اینکه ایمیل ارسال نشده
    factory.EmailService.Verify(
        x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
        Times.Never);
  }



  public class SutFactory : IDisposable
  {
    public ReactifyBlogDbContext DbContext { get; }
    public Mock<UserManager<UserDBO>> UserManager { get; }
    public Mock<SignInManager<UserDBO>> SignInManager { get; }
    public Mock<IEmailService> EmailService { get; } = new Mock<IEmailService>();
    public Mock<IHttpContextAccessorWrapper> HttpContextAccessorWrapper { get; } = new Mock<IHttpContextAccessorWrapper>();
    public IMapper Mapper { get; }

    public SutFactory()
    {
      // AutoMapper
      var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
      Mapper = config.CreateMapper();

      // InMemory DbContext
      var options = new DbContextOptionsBuilder<ReactifyBlogDbContext>()
          .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Db جداگانه برای هر تست
          .Options;
      DbContext = new ReactifyBlogDbContext(options);

      // Mock UserManager
      var userStore = new Mock<IUserStore<UserDBO>>();
      UserManager = new Mock<UserManager<UserDBO>>(
          userStore.Object,
          null, null, null, null, null, null, null, null
      );

      // Mock SignInManager
      var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
      var claimsFactory = new Mock<IUserClaimsPrincipalFactory<UserDBO>>();
      SignInManager = new Mock<SignInManager<UserDBO>>(
          UserManager.Object,
          contextAccessor.Object,
          claimsFactory.Object,
          null, null, null, null
      );
    }

    public async Task<AuthenticationService> CreateSut()
    {
      await SeedDataAsync();

      return new AuthenticationService(
          UserManager.Object,
          SignInManager.Object,
          HttpContextAccessorWrapper.Object,
          EmailService.Object,
          Mapper,
          DbContext
      );
    }

    public async Task SeedDataAsync()
    {
      var seeder = new TestDataSeeder(DbContext);
      await seeder.SeedAllAsync(); // فرض کن SeedAll async است
    }

    public void Dispose()
    {
      DbContext.Dispose(); // همین باعث پاک شدن InMemory DB بعد از هر تست می‌شود
    }
  }
}