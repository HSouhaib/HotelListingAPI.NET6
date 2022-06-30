using HotelListing.API.Models;
namespace HotelListing.API.Core.CoreContracts;

public interface ICountriesRepository : IGenericRepository<Country>
{
   Task<Country> GetDetails(int id);

}
