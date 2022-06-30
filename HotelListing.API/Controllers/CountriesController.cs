using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using HotelListing.API.Core.CoreContracts;
using HotelListing.API.Core.CoreModelsDTO.Country;
using HotelListing.API.Core.CoreModels;
using HotelListing.API.Core.CoreExceptions;

namespace HotelListing.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
  
   public class CountriesController : ControllerBase
   {
      private readonly IMapper _mapper;
      private readonly ICountriesRepository _countriesRepository;
      private readonly ILogger<CountriesController> _logger;

      public CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger)
      {
         _mapper = mapper;
         _countriesRepository = countriesRepository;
         _logger = logger;
      }

      // GET: api/Countries/All
      [HttpGet("All")]
      [EnableQuery]
      public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetAllCountries()
      {
         var countries = await _countriesRepository.GetAllAsync<GetCountryDTO>();
         //var countries = await _countriesRepository.GetAllAsync();
         //var records = _mapper.Map<List<GetCountryDTO>>(countries);
         return Ok(countries);
         //return Ok(await _context.Countries.ToListAsync(); return 200 request status
      }
      // GET: api/Countries/GetAll
      [HttpGet("GetAll")]
     
      public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
      {
         var countries = await _countriesRepository.GetAllAsync();
         var records = _mapper.Map<List<GetCountryDTO>>(countries);
         return Ok(records);
         //return Ok(await _context.Countries.ToListAsync(); return 200 request status
      }

      // GET: api/Countries?StartIndex=0&pageSize=25&PageNumber=1
      [HttpGet]
      public async Task<ActionResult<PageResult<GetCountryDTO>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
      {
         var pagedCountriesResult = await _countriesRepository.GetAllAsync<GetCountryDTO>(queryParameters);
         //var records = _mapper.Map<List<GetCountryDTO>>(countries);
         return Ok(pagedCountriesResult);
         //return Ok(await _context.Countries.ToListAsync(); return 200 request status
      }

      // GET: api/Countries/5
      [HttpGet("{id}")]
      public async Task<ActionResult<GetCountryDetailsDTO>> GetCountry(int id)
      {
        
         var country = await _countriesRepository.GetDetails(id);
         return Ok(country);
         //var country = await _countriesRepository.GetDetails(id);

         //if (country == null)
         //{
         //   //_logger.LogWarning($"No Record found in {nameof(GetCountry)} with Id: {id}.");
         //   //return NotFound("Country is Not Listed");
         //   throw new NotFoundException(nameof(GetCountry), id);
         //}

         //var record = _mapper.Map<GetCountryDetailsDTO>(country);

         //return record;
      }

      // PUT: api/Countries/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
      {
         //if (id != updateCountryDTO.Id)
         //{
         //   return BadRequest("Unmatched COUNTRY ID");
         //}

         ////_context.Entry(updateCountryDTO).State = EntityState.Modified;
         //var country = await _countriesRepository.GetAsync(id);
         //if (country is null)
         //{
         //   throw new NotFoundException(nameof(GetCountries), id);
         //}

         //_mapper.Map(updateCountryDTO, country);

         try
         {
            await _countriesRepository.UpdateAsync(id, updateCountryDTO);
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!await CountryExists(id))
            {
               return NotFound();
            }
            else
            {
               throw;
            }
         }

         return NoContent();
      }

      // POST: api/Countries
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      [Authorize(Roles = "Administrator")]
      public async Task<ActionResult<GetCountryDTO>> PostCountry(CreateCountryDTO createcountryDTO)
      {
         //preventing over-posting
         //var countryOld = new Country
         //{
         //   Name = CreatecountryDTO.Name,
         //   ShortName = CreatecountryDTO.ShortName
         //};

         //preventing over-posting and converting from one datatype to other.
         //var country = _mapper.Map<Country>(CreatecountryDTO);

        var country =await  _countriesRepository.AddAsync<CreateCountryDTO, GetCountryDTO>(createcountryDTO);

         return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
      }

      // DELETE: api/Countries/5
      [HttpDelete("{id}")]
      [Authorize(Roles ="Administrator")]
      public async Task<IActionResult> DeleteCountry(int id)
      {
         //if (_countriesRepository is null)
         //{
         //   return NotFound();
         //}
         //var country = await _countriesRepository.GetAsync(id);
         //if (country == null)
         //{
         //   throw new NotFoundException(nameof(GetCountries), id);
         //}
         await _countriesRepository.DeleteAsync(id);

         return NoContent();
         //return 204 request status 
      }

      private async Task<bool> CountryExists(int id)
      {
         return await  _countriesRepository.Exist(id);
         // find a specific country that match the id we're looking for. 
      }
   }
}
