using Microsoft.AspNetCore.Identity;

namespace ReactifyBlog.Data.Models
{
    public class ReactifyBlogUser : IdentityUser<Guid>
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
