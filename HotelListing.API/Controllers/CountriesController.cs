using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models;
using HotelListing.API.ModelsDTO.Country;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
  
   public class CountriesController : ControllerBase
   {
      private readonly IMapper _mapper;
      private readonly ICountriesRepository _countriesRepository;

      public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
      {
         _mapper = mapper;
         _countriesRepository = countriesRepository;
      }

      // GET: api/Countries
      [HttpGet]
      public async Task<ActionResult<IEnumerable<GetCountryDTO>>> GetCountries()
      {
         var countries = await _countriesRepository.GetAllAsync();
         var records = _mapper.Map<List<GetCountryDTO>>(countries);
         return Ok(records);
         //return Ok(await _context.Countries.ToListAsync(); return 200 request status
      }

      // GET: api/Countries/5
      [HttpGet("{id}")]
      public async Task<ActionResult<GetCountryDetailsDTO>> GetCountry(int id)
      {
         var country = await _countriesRepository.GetDetails(id);

         if (country == null)
         {
            return NotFound("Country is Not Listed");
         }

         var record = _mapper.Map<GetCountryDetailsDTO>(country);

         return record;
      }

      // PUT: api/Countries/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      [Authorize(Roles = "Administrator")]
      public async Task<IActionResult> PutCountry(int id, UpdateCountryDTO updateCountryDTO)
      {
         if (id != updateCountryDTO.Id)
         {
            return BadRequest("Unmatched COUNTRY ID");
         }

         //_context.Entry(updateCountryDTO).State = EntityState.Modified;
         var country = await _countriesRepository.GetAsync(id);
         if (country is null)
         {
            return NotFound();
         }

         _mapper.Map(updateCountryDTO, country);

         try
         {
            await _countriesRepository.UpdateAync(country);
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
      public async Task<ActionResult<Country>> PostCountry(CreateCountryDTO CreatecountryDTO)
      {
         //preventing over-posting
         //var countryOld = new Country
         //{
         //   Name = CreatecountryDTO.Name,
         //   ShortName = CreatecountryDTO.ShortName
         //};

         //preventing over-posting and converting from one datatype to other.
         var country = _mapper.Map<Country>(CreatecountryDTO);

        await  _countriesRepository.AddAsync(country);

         return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
      }

      // DELETE: api/Countries/5
      [HttpDelete("{id}")]
      [Authorize(Roles ="Administrator")]
      public async Task<IActionResult> DeleteCountry(int id)
      {
         if (_countriesRepository is null)
         {
            return NotFound();
         }
         var country = await _countriesRepository.GetAsync(id);
         if (country == null)
         {
            return NotFound();
         }

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
