﻿namespace HotelListing.API.ModelsDTO.Users;

public class AuthResponseDTO
{
   public string UserId { get; set; }
   public string Token { get; set; }
   public string RefreshToken { get; set; }
}
