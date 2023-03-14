using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Konsten.Models;
using Konsten.Data;

namespace Konsten.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        //för rootpath
        private string? wwwRootPath;
        public ArtworkController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = _hostEnvironment.WebRootPath;
        }

        // GET: Artwork
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Artwork.Include(a => a.ArtistName);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: Art with no editing options
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Artwork.Include(a => a.ArtistName);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Artwork/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artwork == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artwork
                .Include(a => a.ArtistName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // GET: Artwork/Create
        public IActionResult Create()
        {
            ViewData["ArtistNameId"] = new SelectList(_context.Set<ArtistName>(), "Id", "TheArtist");
            return View();
        }

        // POST: Artwork/Create original
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        /* For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtName,ArtYear,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,ImageFile,AltText,ArtistNameId")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistNameId"] = new SelectList(_context.Set<ArtistName>(), "Id", "TheArtist", artwork.ArtistNameId);
            return View(artwork);
        } */

        // POST: Artwork/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistNameId,ArtName,ArtYear,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,AltText,ImageFile")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                //attached image or not
                if (artwork.ImageFile != null)
                {
                    //save image in wwwroot w 2 new variables to make unique filenames
                    string fileName = Path.GetFileNameWithoutExtension(artwork.ImageFile.FileName);
                    string extension = Path.GetExtension(artwork.ImageFile.FileName);

                    //remove space and add timestamp    
                    artwork.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension;
                    //where to store image    
                    string path = Path.Combine(wwwRootPath + "/imageupload/", fileName);
                    //store image
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await artwork.ImageFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    artwork.ImageName = null;
                }

                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["ArtistNameId"] = new SelectList(_context.Set<ArtistName>(), "Id", "TheArtist", artwork.ArtistNameId);
            return View(artwork);
        }

        // GET: Artwork/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artwork == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artwork.FindAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            ViewData["ArtistNameId"] = new SelectList(_context.Set<ArtistName>(), "Id", "TheArtist", artwork.ArtistNameId);
            return View(artwork);
        }

        // POST: Artwork/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtName,ArtYear,ArtTechnique,ArtPrice,ArtWidth,ArtHeight,ImageName,AltText,ArtistNameId")] Artwork artwork)
        {
            if (id != artwork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artwork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtworkExists(artwork.Id))
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
            ViewData["ArtistNameId"] = new SelectList(_context.Set<ArtistName>(), "Id", "TheArtist", artwork.ArtistNameId);
            return View(artwork);
        }

        // GET: Artwork/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artwork == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artwork
                .Include(a => a.ArtistName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // POST: Artwork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artwork == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artwork'  is null.");
            }
            var artwork = await _context.Artwork.FindAsync(id);
            /*delete image from wwwroot */
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "imageupload", artwork.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            if (artwork != null)
            {
                _context.Artwork.Remove(artwork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtworkExists(int id)
        {
            return (_context.Artwork?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
