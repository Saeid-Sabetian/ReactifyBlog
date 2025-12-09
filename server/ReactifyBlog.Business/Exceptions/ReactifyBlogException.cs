using System.Net;

namespace ReactifyBlog.Business.Exceptions
{
    public class ReactifyBlogException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public ReactifyBlogException(string errorCode, int statusCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
