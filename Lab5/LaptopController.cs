using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiuLab5.Data;
using TiuLab5.Models;

namespace TiuLab5.Controllers
{
    public class LaptopController : Controller
    {
        private readonly SklepContext _context;

        public LaptopController(SklepContext context)
        {
            _context = context;
        }

        // GET: Laptop
        public async Task<IActionResult> Index()
        {
            var sklepContext = _context.Laptopy.Include(l => l.Podzespol);
            return View(await sklepContext.ToListAsync());
        }

        // GET: Laptop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptopy
                .Include(l => l.Podzespol)
                .FirstOrDefaultAsync(m => m.LaptopId == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // GET: Laptop/Create
        public IActionResult Create()
        {
            ViewData["PodzespolId"] = new SelectList(_context.Podzespoly, "PodzespolId", "Nazwa");
            return View();
        }

        // POST: Laptop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LaptopId,Model,Ocena,Dostepny,PodzespolId")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(laptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PodzespolId"] = new SelectList(_context.Podzespoly, "PodzespolId", "Nazwa", laptop.PodzespolId);
            return View(laptop);
        }

        // GET: Laptop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptopy.FindAsync(id);
            if (laptop == null)
            {
                return NotFound();
            }
            ViewData["PodzespolId"] = new SelectList(_context.Podzespoly, "PodzespolId", "Nazwa", laptop.PodzespolId);
            return View(laptop);
        }

        // POST: Laptop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LaptopId,Model,Ocena,Dostepny,PodzespolId")] Laptop laptop)
        {
            if (id != laptop.LaptopId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(laptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LaptopExists(laptop.LaptopId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PodzespolId"] = new SelectList(_context.Podzespoly, "PodzespolId", "Nazwa", laptop.PodzespolId);
            return View(laptop);
        }

        // GET: Laptop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var laptop = await _context.Laptopy
                .Include(l => l.Podzespol)
                .FirstOrDefaultAsync(m => m.LaptopId == id);
            if (laptop == null)
            {
                return NotFound();
            }

            return View(laptop);
        }

        // POST: Laptop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var laptop = await _context.Laptopy.FindAsync(id);
            if (laptop != null)
            {
                _context.Laptopy.Remove(laptop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LaptopExists(int id)
        {
            return _context.Laptopy.Any(e => e.LaptopId == id);
        }
    }
}
