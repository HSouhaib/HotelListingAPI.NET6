using AutoMapper;
using HotelListing.API.Core.CoreContracts;
using HotelListing.API.Core.CoreModels;
using HotelListing.API.Data;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.CoreRepository
{
   public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
   {
      public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
      {

      }
   }
}
