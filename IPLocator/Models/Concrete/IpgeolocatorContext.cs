//using System;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;

//namespace IPLocator.Models;

//public partial class IPLocatorContext : IdentityDbContext
//{
//    public IPLocatorContext()
//    {
//    }

//    public IPLocatorContext(DbContextOptions<IPLocatorContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<CountryIPBlock> CountryIpblocks { get; set; }

//    public virtual DbSet<CountryLanguageCode> CountryLanguageCodes { get; set; }

//    public virtual DbSet<Tldlist> Tldlists { get; set; }

//    public virtual DbSet<UserResponseCount> UserResponseCounts { get; set; }
//    public virtual DbSet<UserTokens> UserTokens { get; set; }
//    public virtual DbSet<UserStatus> UserStatuses { get; set; }

//    public virtual DbSet<Status> Status { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        => optionsBuilder.UseSqlServer("Host=localhost;Database=IPLocator;Username=postgres;Password=master");

//    //protected override void OnModelCreating(ModelBuilder modelBuilder)
//    //{
//    //    base.OnModelCreating(modelBuilder);

//    //    modelBuilder.Entity<CountryIpblock>(entity =>
//    //    {
//    //        entity.ToTable("CountryIPBlocks");

//    //        entity.Property(e => e.Cidrmask).HasColumnName("CIDRMask");
//    //    });

//    //    modelBuilder.Entity<CountryLanguageCode>(entity =>
//    //    {
//    //        entity.HasKey(e => e.Id).HasName("CountryLanguageCodes_pkey");

//    //        entity.Property(e => e.LanguageCode)
//    //            .HasMaxLength(200)
//    //            .HasColumnName("languageCode");
//    //        entity.Property(e => e.TldlistId).HasColumnName("TLDListId");

//    //        entity.HasOne(d => d.Tldlist).WithMany(p => p.CountryLanguageCodes)
//    //            .HasForeignKey(d => d.TldlistId)
//    //            .HasConstraintName("TLDList_fkey");
//    //    });

//    //    modelBuilder.Entity<Tldlist>(entity =>
//    //    {
//    //        entity.ToTable("TLDList");

//    //        entity.Property(e => e.Tld).HasColumnName("TLD");
//    //    });

//    //    modelBuilder.Entity<UserResponseCount>(entity =>
//    //    {
//    //        entity.ToTable("UserResponseCount");

//    //        entity.HasIndex(e => e.UserId, "IX_UserResponseCount_UserId");

//    //        entity.Property(e => e.Id).ValueGeneratedNever();

//    //    });

//    //    OnModelCreatingPartial(modelBuilder);
//    //}

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
