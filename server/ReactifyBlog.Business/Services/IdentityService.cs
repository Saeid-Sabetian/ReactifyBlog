using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Data.Models;
using System.Security.Cryptography;
using ReactifyBlog.Data.Constants;

namespace ReactifyBlog.Business.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserDBO> _userManager;
        private readonly SignInManager<UserDBO> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<UserDBO> userManager, SignInManager<UserDBO> signInManager, IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<bool> RegisterUserAsync(RegisterRequest request)
        {
            var user = new UserDBO();
            IdentityResult result = new IdentityResult();
            try
            {
                user = _mapper.Map<UserDBO>(request);
                result = await _userManager.CreateAsync(user, request.Password);
            }
            catch (Exception ex)
            {
                var exm = ex;
            }

            if (result.Succeeded)
            {
                string sevenDigitCode = GenerateSecureSevenDigitCode();
                user.ConfirmEmailCode = sevenDigitCode;
                user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);

                await _userManager.UpdateAsync(user);

                if (user.Email != null)
                {
                    await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Your confirmation code is: {sevenDigitCode}");
                }
                await _userManager.AddToRoleAsync(user, RoleConstants.UserRole);

                return true;
            }
            return false;
        }

        public async Task<bool> LoginUserAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: request.RememberMe);
                return true;
            }
            return false;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return false;
            }

            user.NickName = request.NickName;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            string sevenDigitCode = GenerateSecureSevenDigitCode();
            user.ConfirmEmailCode = sevenDigitCode;
            user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user); // Persist the code and expiration

            if (user.Email != null)
            {
                await _emailService.SendEmailAsync(user.Email, "Password Recovery", $"Your password recovery code is: {sevenDigitCode}");
            }
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null || user.ConfirmEmailCode == null || user.ConfirmEmailExpiration == null || user.ConfirmEmailExpiration < DateTimeOffset.UtcNow || user.ConfirmEmailCode != request.Token)
            {
                return false;
            }

            user.EmailConfirmed = true;
            user.ConfirmEmailCode = null;
            user.ConfirmEmailExpiration = null;
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task RefreshTokenAsync(string refreshToken)
        {
            // In a real application, you would validate the refresh token against a stored token (e.g., in a database).
            // If valid, you would revoke the old refresh token, generate new authentication and refresh tokens,
            // and issue a new authentication cookie.
            // For this example, we'll just re-sign in the user if the refresh token is considered valid (e.g., by checking a mock store).
            // This part requires a proper refresh token storage and validation mechanism.
            await Task.CompletedTask;
            throw new NotImplementedException("Refresh token logic needs to be implemented with proper token storage and validation.");
        }

        private string GenerateSecureSevenDigitCode()
        {
            byte[] randomNumber = new byte[4]; // 4 bytes can represent a number up to 2^32 - 1
            RandomNumberGenerator.Fill(randomNumber);
            int code = BitConverter.ToInt32(randomNumber, 0) % 10000000; // Limit to 7 digits
            if (code < 1000000) // Ensure it's always 7 digits (e.g., 0-padded if necessary)
            {
                code += 1000000; // Add 1,000,000 to ensure it's a 7-digit number
            }
            return code.ToString("D7"); // Format to ensure leading zeros if needed
        }
    }
}
