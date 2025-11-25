using Microsoft.AspNetCore.Identity;

namespace ReactifyBlog.Data.Models
{
    public class UserDBO : IdentityUser<Guid>
    {
        public string NickName { get; set; }
        public string? ConfirmEmailCode { get; set; }
        public DateTimeOffset? ConfirmEmailExpiration { get; set; }
    }
}
