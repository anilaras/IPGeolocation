using IPLocator.Models;

namespace IPLocator.Data.Repository.Abstracts
{
    public interface IUserResponseCountRepository : IGenericRepository<UserResponseCount>
    {
        bool isUserHasAnyResponseBefore(Guid user);
    }
}
