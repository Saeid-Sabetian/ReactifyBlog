using Microsoft.AspNetCore.Identity;

namespace ReactifyBlog.Data.Models
{
    public class UserDBO : IdentityUser<long>
    {
        public string NickName { get; set; }
        public string? ConfirmEmailCode { get; set; }
        public DateTimeOffset? ConfirmEmailExpiration { get; set; }

		//Navigation Properties
		public RefreshTokenDBO? RefreshToken { get; set; }
	}
}