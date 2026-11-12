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
    public class PodzespolController : Controller
    {
        private readonly SklepContext _context;

        public PodzespolController(SklepContext context)
        {
            _context = context;
        }

        // GET: Podzespol
        public async Task<IActionResult> Index()
        {
            var podzespoly = await _context.Podzespoly
        .Include(p => p.Laptopy)
        .ToListAsync();
            return View(await _context.Podzespoly.ToListAsync());
        }

        // GET: Podzespol/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var podzespol = await _context.Podzespoly
                .FirstOrDefaultAsync(m => m.PodzespolId == id);
            if (podzespol == null)
            {
                return NotFound();
            }

            return View(podzespol);
        }

        // GET: Podzespol/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Podzespol/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PodzespolId,Nazwa")] Podzespol podzespol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(podzespol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(podzespol);
        }

        // GET: Podzespol/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var podzespol = await _context.Podzespoly.FindAsync(id);
            if (podzespol == null)
            {
                return NotFound();
            }
            return View(podzespol);
        }

        // POST: Podzespol/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PodzespolId,Nazwa")] Podzespol podzespol)
        {
            if (id != podzespol.PodzespolId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(podzespol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PodzespolExists(podzespol.PodzespolId))
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
            return View(podzespol);
        }

        // GET: Podzespol/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var podzespol = await _context.Podzespoly
                .FirstOrDefaultAsync(m => m.PodzespolId == id);
            if (podzespol == null)
            {
                return NotFound();
            }

            return View(podzespol);
        }

        // POST: Podzespol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var podzespol = await _context.Podzespoly.FindAsync(id);
            if (podzespol != null)
            {
                _context.Podzespoly.Remove(podzespol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PodzespolExists(int id)
        {
            return _context.Podzespoly.Any(e => e.PodzespolId == id);
        }
    }
}
