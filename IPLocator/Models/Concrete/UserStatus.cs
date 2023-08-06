using IPLocator.Models.Concrete;
using Microsoft.AspNetCore.Identity;

namespace IPLocator.Models
{
    public class UserStatus : BaseEntity
    {
        public int StatusId { get; set; }
        public string IdentityUserId { get; set; }
    }
}
