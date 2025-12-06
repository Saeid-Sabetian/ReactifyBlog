namespace ReactifyBlog.Business.Constants.ErrorConstants.Exceptions
{
    public static class AuthServiceErrorConstants
    {
        // Register
        public const string UserExistsErrorCode = "AUTH_REG_001";
        public const string UserExistsErrorMessage = "User with this email already exists.";

        public const string RegisterFailedErrorCode = "AUTH_REG_002";
        public const string RegisterFailedErrorMessage = "User registration failed.";

        // Login
        public const string InvalidCredentialsErrorCode = "AUTH_LOGIN_001";
        public const string InvalidCredentialsErrorMessage = "Invalid login credentials.";

        public const string UserNotConfirmedErrorCode = "AUTH_LOGIN_002";
        public const string UserNotConfirmedErrorMessage = "User email not confirmed.";

        public const string LoginFailedErrorCode = "AUTH_LOGIN_003";
        public const string LoginFailedErrorMessage = "Login failed.";

        public const string LockedOutErrorCode = "AUTH_LOGIN_004";
        public const string LockedOutErrorMessage = "User account locked out.";

        // Change Password
        public const string ChangePasswordFailedErrorCode = "AUTH_CHPWD_001";
        public const string ChangePasswordFailedErrorMessage = "Failed to change password.";

        public const string InvalidCurrentPasswordErrorCode = "AUTH_CHPWD_002";
        public const string InvalidCurrentPasswordErrorMessage = "Invalid current password.";

        // Update User
        public const string UpdateUserFailedErrorCode = "AUTH_UPD_001";
        public const string UpdateUserFailedErrorMessage = "Failed to update user information.";

        public const string UserNotFoundErrorCode = "AUTH_UPD_002";
        public const string UserNotFoundErrorMessage = "User not found.";

        // Recover Password
        public const string RecoverPasswordFailedErrorCode = "AUTH_RECPWD_001";
        public const string RecoverPasswordFailedErrorMessage = "Password recovery failed.";

        public const string ResetPasswordTokenInvalidErrorCode = "AUTH_RECPWD_002";
        public const string ResetPasswordTokenInvalidErrorMessage = "Invalid or expired password reset token.";

        // Confirm Email
        public const string ConfirmEmailFailedErrorCode = "AUTH_CONFEMAIL_001";
        public const string ConfirmEmailFailedErrorMessage = "Failed to confirm email.";

        public const string InvalidEmailConfirmationCodeErrorCode = "AUTH_CONFEMAIL_002";
        public const string InvalidEmailConfirmationCodeErrorMessage = "Invalid email confirmation code.";

        // Logout
        public const string LogoutFailedErrorCode = "AUTH_LOGOUT_001";
        public const string LogoutFailedErrorMessage = "Logout failed.";

        // Refresh Token
        public const string InvalidRefreshTokenErrorCode = "AUTH_REFR_001";
        public const string InvalidRefreshTokenErrorMessage = "Invalid or expired refresh token.";

        public const string RefreshTokenGenerationFailedErrorCode = "AUTH_REFR_002";
        public const string RefreshTokenGenerationFailedErrorMessage = "Failed to generate new refresh token.";
    }
}
