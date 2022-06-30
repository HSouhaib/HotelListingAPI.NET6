using HotelListing.API.Core.CoreModelsDTO.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.CoreContracts;

public interface IAuthManager
{
   Task<IEnumerable<IdentityError>> RegisterUser(ApiUserDTO userDto);
   Task<AuthResponseDTO> LoginUser(LoginDTO userDto);
   Task<string> CreateRefreshToken();
   Task<AuthResponseDTO> VerifyRefreshToken(AuthResponseDTO request);
}
