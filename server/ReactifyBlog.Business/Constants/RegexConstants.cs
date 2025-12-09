namespace ReactifyBlog.Business.Constants;

public class RegexConstants
{
  // =========================
  // Password Patterns
  // =========================

  public const string PasswordLowercase = "[a-z]";

  public const string PasswordUppercase = "[A-Z]";

  public const string PasswordDigit = "[0-9]";

  public const int PasswordMinLength = NumericConstants.Six;

  public const string PasswordSpecialCharacter = @"[!@#$%^&*(),.?""{}|<>[\]\\/'`;:\-_=+~]";
}
