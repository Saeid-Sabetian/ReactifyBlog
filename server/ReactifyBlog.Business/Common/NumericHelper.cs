using System.Security.Cryptography;

namespace ReactifyBlog.Business.Common;

public static class NumericHelper
{
  public static int GenerateRandomInt(int from, int to)
  {
    return RandomNumberGenerator.GetInt32(from, to);
  }
}
