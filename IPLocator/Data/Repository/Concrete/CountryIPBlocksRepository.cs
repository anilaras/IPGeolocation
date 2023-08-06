using IPLocator.Models;
using IPLocator.Data.Repository.Abstracts;

namespace IPLocator.Data.Repository.Concrete
{
    public class CountryIPBlocksRepository : GenericRepository<CountryIPBlock>, ICountryIPBlocksRepository
    {
        public CountryIPBlocksRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
