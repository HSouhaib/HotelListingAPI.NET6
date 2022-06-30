using HotelListing.API.Core.CoreModelsDTO.Hotels;

namespace HotelListing.API.Core.CoreModelsDTO.Country;

public class GetCountryDetailsDTO : BaseCountryDTO
{
   public int Id { get; set; }
   public List<GetHotelDTO> Hotels { get; set; }
}
