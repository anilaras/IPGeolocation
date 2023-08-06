using IPLocator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IPLocator.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public virtual DbSet<CountryIPBlock> CountryIpblocks { get; set; }

        public virtual DbSet<CountryLanguageCode> CountryLanguageCodes { get; set; }

        public virtual DbSet<Tldlist> Tldlists { get; set; }

        public virtual DbSet<UserResponseCount> UserResponseCounts { get; set; }
        public virtual DbSet<UserTokens> UserTokens { get; set; }
        public virtual DbSet<UserStatus> UserStatuses { get; set; }

        public virtual DbSet<Status> Status { get; set; }
    }

   
}