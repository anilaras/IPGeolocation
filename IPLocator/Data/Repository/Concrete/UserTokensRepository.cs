using IPLocator.Models;
using IPLocator.Data.Repository.Abstracts;

namespace IPLocator.Data.Repository.Concrete
{
    public class UserTokensRepository : GenericRepository<UserTokens>, IUserTokensRepository
    {
        public UserTokensRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
