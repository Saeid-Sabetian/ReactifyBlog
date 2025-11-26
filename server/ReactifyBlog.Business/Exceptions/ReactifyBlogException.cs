using System.Net;

namespace ReactifyBlog.Business.Exceptions
{
    public class ReactifyBlogException : Exception
    {
        public string ErrorCode { get; }
        public string ServiceName { get; }
        public int StatusCode { get; }

        public ReactifyBlogException(string errorCode, string serviceName, int statusCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            ServiceName = serviceName;
            StatusCode = statusCode;
        }
    }
}
