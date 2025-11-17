using System.ComponentModel.DataAnnotations;

namespace ReactifyBlog.Data.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public Guid UserId { get; set; }
        public ReactifyBlogUser User { get; set; } = default!;
    }
}
