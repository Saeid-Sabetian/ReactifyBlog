namespace ReactifyBlog.Business.DTOs
{
    public class ErrorInfo
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Service { get; set; }
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }

    public class AppResponse<T>
    {
        public T? Data { get; set; }
        public ErrorInfo? Error { get; set; }
    }
}
