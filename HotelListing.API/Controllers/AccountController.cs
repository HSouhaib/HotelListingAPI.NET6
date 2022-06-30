
using HotelListing.API.Core.CoreContracts;
using HotelListing.API.Core.CoreModelsDTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
   private readonly IAuthManager _authManager;
   private readonly ILogger<AccountController> _logger;

   public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
   {
      _authManager = authManager;
      _logger = logger;
   }

   //POST: api/Account/register
   [HttpPost]
   [Route("register")]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<IActionResult> Register([FromBody] ApiUserDTO apiUserDto)
   {
      _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");

      var errors = await _authManager.RegisterUser(apiUserDto);


      if (errors.Any())
      {
         foreach (var error in errors)
         {
            ModelState.AddModelError(error.Code, error.Description);
         }
         return BadRequest(ModelState);
      }
      return Ok();
   }

   //POST: api/Account/login
   [HttpPost]
   [Route("login")]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
   {
      _logger.LogInformation($"Login Attempt for {loginDto.Email}");


      var AuthResponse = await _authManager.LoginUser(loginDto);

      if (AuthResponse is null)
      {
         return Unauthorized();
      }
      return Ok(AuthResponse);
   }

   //POST: api/Account/refreshToken
   [HttpPost]
   [Route("refreshToken")]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDTO request)
   {
      var AuthResponse = await _authManager.VerifyRefreshToken(request);

      if (AuthResponse is null)
      {
         return Unauthorized();
      }
      return Ok(AuthResponse);
   }
}

