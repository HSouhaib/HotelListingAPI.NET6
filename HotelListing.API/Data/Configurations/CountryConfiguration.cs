using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
   public class CountryConfiguration : IEntityTypeConfiguration<Country>
   {
      public void Configure(EntityTypeBuilder<Country> builder)
      {
         builder.HasData
      (
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
      }
   }
}
