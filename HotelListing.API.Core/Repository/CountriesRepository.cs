using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.CoreContracts;
using HotelListing.API.Core.CoreExceptions;
using HotelListing.API.Core.CoreModelsDTO.Country;
using HotelListing.API.Data;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Core.CoreRepository;

public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
{
   private readonly HotelListingDbContext _context;
   private readonly IMapper _mapper;

   public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
   {
      _context = context;
      _mapper = mapper;
   }
   public async Task<GetCountryDTO> GetDetails(int id)
   {
      var country = await _context.Countries.Include(c => c.Hotels)
         .ProjectTo<GetCountryDTO>(_mapper.ConfigurationProvider)
         .FirstOrDefaultAsync(c => c.Id == id);

      if (country is null)
      {
         throw new NotFoundException(nameof(GetDetails), id);
      }

      return country;
   }

   Task<Country> ICountriesRepository.GetDetails(int id)
   {
      throw new NotImplementedException();
   }
}
