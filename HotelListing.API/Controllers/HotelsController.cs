using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models;
using HotelListing.API.Contracts;
using AutoMapper;
using HotelListing.API.ModelsDTO.Hotels;

namespace HotelListing.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class HotelsController : ControllerBase
   {
      private readonly IHotelsRepository _hotelsRepository;
      private readonly IMapper _mapper;

      public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
      {

         _hotelsRepository = hotelsRepository;
         _mapper = mapper;
      }

      // GET: api/Hotels
      [HttpGet]
      public async Task<ActionResult<IEnumerable<GetHotelDTO>>> GetHotels()
      {
         var hotels = await _hotelsRepository.GetAllAsync(); ;
         return _mapper.Map<List<GetHotelDTO>>(hotels);

      }

      // GET: api/Hotels/5
      [HttpGet("{id}")]
      public async Task<ActionResult<GetHotelDTO>> GetHotel(int id)
      {
         var hotel = await _hotelsRepository.GetAsync(id);

         if (hotel == null)
         {
            return NotFound("Hotel is not listed");
         }

         return _mapper.Map<GetHotelDTO>(hotel); ;
      }

      // PUT: api/Hotels/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutHotel(int id, GetHotelDTO hotelDto)
      {
         if (id != hotelDto.Id)
         {
            return BadRequest("Unmatched HOTEL ID");
         }
         var hotel = await _hotelsRepository.GetAsync(id);

         if (hotel == null)
         {
            return NotFound("The hotel you're looking for doesn't exist, please try another one");
         }
         _mapper.Map(hotelDto,hotel);

         try
         {
            await _hotelsRepository.UpdateAync(hotel);
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!await HotelExists(id))
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

      // POST: api/Hotels
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDTO hotelDto)
      {
         var hotel = _mapper.Map<Hotel>(hotelDto);

         await _hotelsRepository.AddAsync(hotel);

         return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
      }

      // DELETE: api/Hotels/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteHotel(int id)
      {
         var hotel = await _hotelsRepository.GetAsync(id);

         if (hotel == null)
         {
            return NotFound();
         }

         await _hotelsRepository.DeleteAsync(id);

         return NoContent();
      }

      private async Task<bool> HotelExists(int id)
      {
         return await _hotelsRepository.Exist(id);
         // find a specific hotel that match the id we're looking for. 
      }
   }
}
