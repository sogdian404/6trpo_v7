using _6_webApi.Models;

using Microsoft.AspNetCore.Mvc;

namespace _6_webApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HotelController : ControllerBase
    {
        readonly WebApiDatabaseContext _db = new();
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? city, [FromQuery] decimal? minRating)
        {
            IQueryable<Hotel> query = _db.Hotels;

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(h => h.City == city);
            if (minRating!=null)
                query = query.Where(h => h.Rating >= minRating);
            return Ok(query.ToList());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpPost]
        public IActionResult Create(Hotel hotel)
        {
            if (hotel.Name ==null || hotel.City == null || hotel.Rating ==null||hotel.PricePerNight==null||hotel.IsAvailable==null)
            {
                return BadRequest();
            }
            hotel.Id = 0;
            _db.Hotels.Add(hotel);
            _db.SaveChanges();
            return Created();
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Hotel hotel)
        {
            var hotel2 = _db.Hotels.FirstOrDefault(h => h.Id == id);

            if (hotel2 == null)
                return NotFound();
            if (hotel.Name == null || hotel.City == null || hotel.Rating == null || hotel.PricePerNight == null || hotel.IsAvailable == null)
            {
                return BadRequest();
            }
            hotel2.Name = hotel.Name;
            hotel2.City = hotel.City;
            hotel2.Rating = hotel.Rating;
            hotel2.PricePerNight = hotel.PricePerNight;
            hotel2.IsAvailable = hotel.IsAvailable;
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id:int}")]
        public IActionResult Patch([FromRoute]int id, [FromBody] HotelPatch hotelPatch)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
                return NotFound();
            if(hotelPatch.Name!=null)hotel.Name = hotelPatch.Name;
            if (hotelPatch.City != null) hotel.City = hotelPatch.City;
            if (hotelPatch.Rating != null) hotel.Rating = (decimal)hotelPatch.Rating;
            if (hotelPatch.PricePerNight != null) hotel.PricePerNight = (decimal)hotelPatch.PricePerNight;
            if (hotelPatch.IsAvailable != null) hotel.IsAvailable = (bool)hotelPatch.IsAvailable;

            _db.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var hotel = _db.Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
            return NotFound();
            }
            _db.Hotels.Remove(hotel);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
