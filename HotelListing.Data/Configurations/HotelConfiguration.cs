using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
   public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
   {
      public void Configure(EntityTypeBuilder<Hotel> builder)
      {
         builder.HasData
      (
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
       );
      }
   }
}
