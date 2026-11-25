using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabTiu7.Data;
using LabTiu7.Models;

namespace LabTiu7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LaptopyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Laptopy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Laptop>>> GetLaptopy()
        {
            return await _context.Laptopy.ToListAsync();
        }

        // GET: api/Laptopy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Laptop>> GetLaptop(int id)
        {
            var laptop = await _context.Laptopy.FindAsync(id);
            if (laptop == null) return NotFound();
            return laptop;
        }

        // POST: api/Laptopy
        [HttpPost]
        public async Task<ActionResult<Laptop>> PostLaptop(Laptop laptop)
        {
            _context.Laptopy.Add(laptop);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLaptop), new { id = laptop.Id }, laptop);
        }

        // PUT: api/Laptopy/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaptop(int id, Laptop laptop)
        {
            if (id != laptop.Id) return BadRequest();

            _context.Entry(laptop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Laptopy.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Laptopy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaptop(int id)
        {
            var laptop = await _context.Laptopy.FindAsync(id);
            if (laptop == null) return NotFound();

            _context.Laptopy.Remove(laptop);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}