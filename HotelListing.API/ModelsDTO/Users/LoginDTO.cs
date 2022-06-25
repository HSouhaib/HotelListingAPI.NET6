using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.ModelsDTO.Users;

public class LoginDTO
{
   [Required]
   [EmailAddress]
   public string Email { get; set; }

   [Required]
   [StringLength(15, ErrorMessage = "Your Password is limited from {2} to {1} characters,",
      MinimumLength = 6)]
   public string Password { get; set; }
}
