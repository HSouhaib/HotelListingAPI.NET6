using AutoMapper;
using HotelListing.API.Core.CoreModelsDTO.Country;
using HotelListing.API.Core.CoreModelsDTO.Hotels;
using HotelListing.API.Core.CoreModelsDTO.Users;
using HotelListing.API.Data;
using HotelListing.API.Models;

namespace HotelListing.API.Core.CoreConfiguration;

public class MapperConfig : Profile
{
   public MapperConfig()
   {
      CreateMap<Country, CreateCountryDTO>().ReverseMap();
      CreateMap<Country, GetCountryDTO>().ReverseMap();
      CreateMap<Country, GetCountryDetailsDTO>().ReverseMap();
      CreateMap<Country, UpdateCountryDTO>().ReverseMap();

      CreateMap<Hotel, GetHotelDTO>().ReverseMap();
      CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

      CreateMap<ApiUserDTO, ApiUser>().ReverseMap();

   }
}
