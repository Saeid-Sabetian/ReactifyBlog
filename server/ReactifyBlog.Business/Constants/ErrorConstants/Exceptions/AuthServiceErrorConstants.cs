namespace ReactifyBlog.Business.Constants.ErrorConstants.Exceptions
{
  public static class AuthServiceErrorConstants
  {
    // =========================
    // Register
    // =========================
    public const string RegRequiredEmailErrorCode = "AUTH_REG_001";
    public const string RegRequiredEmailErrorMessage = "Email is required.";

    public const string RegInvalidEmailFormatErrorCode = "AUTH_REG_02";
    public const string RegInvalidEmailFormatErrorMessage = "Invalid email format.";

    public const string RegRequiredPasswordErrorCode = "AUTH_REG_03";
    public const string RegRequiredPasswordErrorMessage = "Password is required.";

    public const string RegInvalidPasswordLengthErrorCode = "AUTH_REG_04";
    public const string RegInvalidPasswordLengthErrorMessage = "Password must be at least 6 characters long";

    public const string RegPasswordMismatchErrorCode = "AUTH_REG_005";
    public const string RegPasswordMismatchErrorMessage = "Passwords do not match.";

    public const string RegPasswordRequiresLowerErrorCode = "AUTH_REG_06";
    public const string RegPasswordRequiresLowerErrorMessage = "Password must contain at least one lowercase letter.";

    public const string RegPasswordRequiresUpperErrorCode = "AUTH_REG_07";
    public const string RegPasswordRequiresUpperErrorMessage = "Password must contain at least one uppercase letter.";
    
    public const string RegPasswordRequiresDigitErrorCode = "AUTH_REG_08";
    public const string RegPasswordRequiresDigitErrorMessage = "Password must contain at least one digit.";

    public const string RegPasswordRequiresSpecialCharErrorCode = "AUTH_REG_09";
    public const string RegPasswordRequiresSpecialCharErrorMessage = "Password must contain at least one special character (e.g., @, !, #).";

    public const string RegisterDuplicateEmailErrorCode = "AUTH_REG_10";
    public const string RegisterDuplicateEmailErrorMessage = "This email is already registered.";

    public const string RegisterFailedErrorCode = "AUTH_REG_11";
    public const string RegisterFailedErrorMessage = "Registration failed. Please try again later.";


    // =========================
    // Login
    // =========================
    public const string LoginRequiredEmailErrorCode = "AUTH_LOGIN_01";
    public const string LoginRequiredEmailErrorMessage = "Email is required.";

    public const string LoginRequiredPasswordErrorCode = "AUTH_LOGIN_02";
    public const string LoginRequiredPasswordErrorMessage = "Password is required.";

    public const string LoginInvalidPasswordLengthErrorCode = "AUTH_LOGIN_03";
    public const string LoginInvalidPasswordLengthErrorMessage = "Password must be at least 6 characters long";

    public const string LoginInvalidCredentialsErrorCode = "AUTH_LOGIN_04";
    public const string LoginInvalidCredentialsErrorMessage = "Invalid login credentials.";

    public const string LoginUserNotConfirmedErrorCode = "AUTH_LOGIN_05";
    public const string LoginUserNotConfirmedErrorMessage = "User email not confirmed.";

    public const string LoginFailedErrorCode = "AUTH_LOGIN_06";
    public const string LoginFailedErrorMessage = "Login failed.";

    public const string LockedOutErrorCode = "AUTH_LOGIN_07";
    public const string LockedOutErrorMessage = "User account is locked. It will unlock in {0} minute(s).";

    // =========================
    // Change Password
    // ========================= 
    public const string ChangePasswordRequiredOldPasswordErrorCode = "AUTH_CHPWD_01";
    public const string ChangePasswordRequiredOldPasswordErrorMessage = "The old password is required.";

    public const string ChangePasswordRequiredNewPasswordErrorCode = "AUTH_CHPWD_02";
    public const string ChangePasswordRequiredNewPasswordErrorMessage = "The new password is required.";

    public const string ChangePasswordNewPasswordLengthErrorCode = "AUTH_CHPWD_03";
    public const string ChangePasswordNewPasswordLengthErrorMessage = "The new password must be at least 6 characters long";

    public const string ChangePasswordNewPasswordRequiresLowerErrorCode = "AUTH_CHPWD_04";
    public const string ChangePasswordNewPasswordRequiresLowerErrorMessage = "The new password must contain at least one lowercase letter.";

    public const string ChangePasswordNewPasswordRequiresUpperErrorCode = "AUTH_CHPWD_05";
    public const string ChangePasswordNewPasswordRequiresUpperErrorMessage = "The new password must contain at least one uppercase letter.";

    public const string ChangePasswordNewPasswordRequiresDigitErrorCode = "AUTH_CHPWD_05";
    public const string ChangePasswordNewPasswordRequiresDigitErrorMessage = "The new password must contain at least one digit.";

    public const string ChangePasswordNewPasswordRequiresSpecialCharErrorCode = "AUTH_CHPWD_06";
    public const string ChangePasswordNewPasswordRequiresSpecialCharErrorMessage = "The new password must contain at least one special character (e.g., @, !, #).";

    public const string ChangePasswordConfirmNewPasswordIsRequiredErrorCode = "AUTH_CHPWD_07";
    public const string ChangePasswordConfirmNewPasswordIsRequiredErrorMessage = "ConfirmNewPassword is required.";

    public const string ChangePasswordConfirmNewPasswordMismatchErrorCode = "AUTH_CHPWD_08";
    public const string ChangePasswordConfirmNewPasswordMismatchErrorMessage = "ConfirmNewPassword do not match.";




    public const string ChangePasswordFailedErrorCode = "AUTH_CHPWD_01";
    public const string ChangePasswordFailedErrorMessage = "Failed to change password.";

    public const string InvalidCurrentPasswordErrorCode = "AUTH_CHPWD_02";
    public const string InvalidCurrentPasswordErrorMessage = "Invalid current password.";

    // =========================
    // Update User
    // ========================= 
    public const string UpdateUserFailedErrorCode = "AUTH_UPD_01";
    public const string UpdateUserFailedErrorMessage = "Failed to update user information.";

    public const string UserNotFoundErrorCode = "AUTH_UPD_02";
    public const string UserNotFoundErrorMessage = "User not found.";

    // =========================
    // Recover Password
    // =========================  
    public const string RecoverPasswordFailedErrorCode = "AUTH_RECPWD_01";
    public const string RecoverPasswordFailedErrorMessage = "Password recovery failed.";

    public const string ResetPasswordTokenInvalidErrorCode = "AUTH_RECPWD_02";
    public const string ResetPasswordTokenInvalidErrorMessage = "Invalid or expired password reset token.";

    // =========================
    // Confirm Email
    // =========================  
    public const string ConfirmEmailRequiredEmailErrorCode = "AUTH_CONFEMAIL_01";
    public const string ConfirmEmailRequiredEmailErrorMessage = "Email is required.";

    public const string ConfirmEmailFormatErrorCode = "AUTH_CONFEMAIL_02";
    public const string ConfirmEmailFormatErrorMessage = "Invalid email format.";

    public const string ConfirmEmailConfirmationCodeLengthErrorCode = "AUTH_CONFEMAIL_03";
    public const string ConfirmEmailConfirmationCodeLengthErrorMessage = "The confirmation code must be a 6-digit number.";

    public const string ConfirmEmailUserDoesNotExistErrorCode = "AUTH_CONFEMAIL_04";
    public const string ConfirmEmailUserDoesNotExistErrorMessage = "User does not exist.";

    public const string ConfirmEmailAlreadyConfirmedErrorCode = "AUTH_CONFEMAIL_05";
    public const string ConfirmEmailAlreadyConfirmedErrorMessage = "The email has already been confirmed.";

    public const string ConfirmEmailExpiredErrorCode = "AUTH_CONFEMAIL_06";
    public const string ConfirmEmailExpiredErrorMessage = "The email confirmation time limit has expired.";

    public const string ConfirmEmailInvalidCodeErrorCode = "AUTH_CONFEMAIL_07";
    public const string ConfirmEmailInvalidCodeErrorMessage = "The email confirmation code is incorrect.";

    // =========================
    // Logout
    // =========================  
    public const string LogoutFailedErrorCode = "AUTH_LOGOUT_01";
    public const string LogoutFailedErrorMessage = "Logout failed.";

    // =========================
    // Refresh Token
    // =========================
    public const string InvalidRefreshTokenErrorCode = "AUTH_REFR_01";
    public const string InvalidRefreshTokenErrorMessage = "Invalid or expired refresh token.";

    public const string RefreshTokenGenerationFailedErrorCode = "AUTH_REFR_02";
    public const string RefreshTokenGenerationFailedErrorMessage = "Failed to generate new refresh token.";
  }
}
