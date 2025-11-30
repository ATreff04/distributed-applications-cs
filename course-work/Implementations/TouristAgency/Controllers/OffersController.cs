using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TouristAgency.Entities;

namespace TouristAgency.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private readonly TouristAgencyDbContext _context;

        public OffersController(TouristAgencyDbContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index(string search, string sortOrder, int page = 1)
        {
            int pageSize = 5;

            var offers = _context.Offers
                .Include(o => o.Destination)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                offers = offers.Where(o =>
                    o.Title.Contains(search) ||
                    o.Destination.Name.Contains(search));
            }

            ViewData["PriceSort"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            offers = sortOrder switch
            {
                "price_asc" => offers.OrderBy(o => o.Price),
                "price_desc" => offers.OrderByDescending(o => o.Price),
                _ => offers.OrderBy(o => o.Id)
            };

            int totalOffers = await offers.CountAsync();
            int totalPages = (int)Math.Ceiling(totalOffers / (double)pageSize);

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            var items = await offers.Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return View(items);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = await _context.Offers
                .Include(o => o.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null)
                return NotFound();

            return View(offer);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var destinations = _context.Destinations
                .Select(d => new
                {
                    d.Id,
                    Text = d.Name + " (" + d.Country + ")"
                })
                .ToList();

            ViewBag.DestinationId = new SelectList(destinations, "Id", "Text");

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DestinationId = new SelectList(
                _context.Destinations
                    .Select(d => new { d.Id, Text = d.Name + " (" + d.Country + ")" }),
                "Id", "Text", offer.DestinationId);

            return View(offer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                return NotFound();

            ViewData["DestinationId"] =
                new SelectList(_context.Destinations, "Id", "Name", offer.DestinationId);

            return View(offer);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Offer offer)
        {
            if (id != offer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DestinationId"] =
                new SelectList(_context.Destinations, "Id", "Name", offer.DestinationId);

            return View(offer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var offer = await _context.Offers
                .Include(o => o.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null)
                return NotFound();

            return View(offer);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);

            if (offer != null)
                _context.Offers.Remove(offer);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.Id == id);
        }
    }
}
