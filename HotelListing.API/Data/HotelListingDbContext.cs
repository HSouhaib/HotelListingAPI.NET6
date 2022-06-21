using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data;
public class HotelListingDbContext : DbContext
{
   public HotelListingDbContext(DbContextOptions options) : base(options)
   {

   }

   public DbSet<Hotel> Hotels { get; set; }
   public DbSet<Country> Countries { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<Country>().HasData(
         new Country
         {
            Id = 1,
            Name = "Egypte",
            ShortName = "EG"

         },
            new Country
            {
               Id = 2,
               Name = "Brazil",
               ShortName = "BZ"

            }, new Country
            {
               Id = 3,
               Name = "Morocco",
               ShortName = "MR"

            }, new Country
            {
               Id = 4,
               Name = "Belgium",
               ShortName = "BG"

            }
            );

      modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
               Id = 1,
               Name = "Brussels Resort and Spa",
               Address = "Brussels",
               CountryId = 4,
               Rating = 4.3
             
            },
            new Hotel
            {
               Id = 2,
               Name = "Meknes Resort and Spa",
               Address = "Meknes",
               CountryId = 3,
               Rating = 4.3

            }, new Hotel
            {
               Id = 3,
               Name = "Egypte Resort and Spa",
               Address = "Charm-Sheikh",
               CountryId = 1,
               Rating = 4.3

            }, new Hotel
            {
               Id = 4,
               Name = "Brazil Resort and Spa",
               Address = "Rio-De-Jenero",
               CountryId = 2,
               Rating = 4.3
            }
            ) ;
   }
}
