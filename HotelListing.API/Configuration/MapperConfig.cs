using AutoMapper;
using HotelListing.API.Models;
using HotelListing.API.ModelsDTO.Country;
using HotelListing.API.ModelsDTO.Hotels;

namespace HotelListing.API.Configuration;

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


   }
}
