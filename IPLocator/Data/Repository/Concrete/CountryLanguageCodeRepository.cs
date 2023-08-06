using IPLocator.Models;
using IPLocator.Data.Repository.Abstracts;

namespace IPLocator.Data.Repository.Concrete
{
    public class CountryLanguageCodeRepository : GenericRepository<CountryLanguageCode>, ICountryLanguageCodeRepository
    {
        public CountryLanguageCodeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
