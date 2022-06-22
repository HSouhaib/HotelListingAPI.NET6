using HotelListing.API.ModelsDTO.Hotels;

namespace HotelListing.API.ModelsDTO.Country;

public class GetCountryDetailsDTO : BaseCountryDTO
{
   public int Id { get; set; }
   public List<GetHotelDTO> Hotels { get; set; }
}
