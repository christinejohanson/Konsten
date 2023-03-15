using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Konsten.Models;
using Konsten.Data;

namespace Konsten.Controllers
{
    public class ArtistNameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistNameController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ArtistName
        public async Task<IActionResult> Index()
        {
            return _context.ArtistName != null ?
                        View(await _context.ArtistName.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ArtistName'  is null.");
        }

        // GET: ArtistName with no editing options

        [Route("/artists")]
        public async Task<IActionResult> List()
        {
            return _context.ArtistName != null ?
            View(await _context.ArtistName.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ArtistName'  is null.");
        }

        // GET: ArtistName/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ArtistName == null)
            {
                return NotFound();
            }

            var artistName = await _context.ArtistName
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistName == null)
            {
                return NotFound();
            }

            return View(artistName);
        }

        // GET: ArtistName/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtistName/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TheArtist")] ArtistName artistName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artistName);
        }

        // GET: ArtistName/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ArtistName == null)
            {
                return NotFound();
            }

            var artistName = await _context.ArtistName.FindAsync(id);
            if (artistName == null)
            {
                return NotFound();
            }
            return View(artistName);
        }

        // POST: ArtistName/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TheArtist")] ArtistName artistName)
        {
            if (id != artistName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistNameExists(artistName.Id))
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
            return View(artistName);
        }

        // GET: ArtistName/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ArtistName == null)
            {
                return NotFound();
            }

            var artistName = await _context.ArtistName
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistName == null)
            {
                return NotFound();
            }

            return View(artistName);
        }

        // POST: ArtistName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ArtistName == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ArtistName'  is null.");
            }
            var artistName = await _context.ArtistName.FindAsync(id);
            if (artistName != null)
            {
                _context.ArtistName.Remove(artistName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistNameExists(int id)
        {
            return (_context.ArtistName?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
