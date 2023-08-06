using IPLocator.Models;
using IPLocator.Data.Repository.Abstracts;

namespace IPLocator.Data.Repository.Concrete
{
    public class UserResponseCountRepository : GenericRepository<UserResponseCount>, IUserResponseCountRepository
    {
        public UserResponseCountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool isUserHasAnyResponseBefore(Guid user)
        {
            return GetAll().Any(d => d.IdentityUserId.Equals(user));
        }
    }
}
