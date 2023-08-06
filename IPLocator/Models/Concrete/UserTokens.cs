
using IPLocator.Models.Concrete;
using Microsoft.AspNetCore.Identity;

namespace IPLocator.Models
{
    public class UserTokens : BaseEntity
    {
        public string Token { get; set; }

        public Guid IdentityUserId { get; set; }
    }
}
