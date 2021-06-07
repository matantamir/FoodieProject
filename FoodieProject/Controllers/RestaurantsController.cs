using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodieProject.Data;
using FoodieProject.Models;

namespace FoodieProject.Controllers
{
    //[Authorize] - in order to make the all function accessble just for authnticated users
    //[Authorize(Roles ="Admin")] - in order to restrict function just for admins
    public class RestaurantsController : Controller
    {
        // Create a Directory in order to save pictures in servere side
        private readonly string restPicDir = Path.Combine(Directory.GetCurrentDirectory(), "Pictures\\Rest");

        private readonly FoodieProjectContext _context;

        public RestaurantsController(FoodieProjectContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var foodieProjectContext = _context.Restaurant.Include(r => r.Address);
            return View(await foodieProjectContext.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            ViewData["Tags"] = new MultiSelectList(_context.Tag, "Id", "Name");
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,AddressId,AveragePrice,PicturePath,Rate,About")] Restaurant restaurant,
            [Bind("Id,City,Street,Number")] Address address,
            [Bind("tagToCare")] List<int> tagToCare,
            // Get also a picture from user
            IFormFile myFile
            )
        {
            // If we have got an image
            if(myFile != null)
            {
                restaurant.PicturePath = ImageUpload(myFile);
            }

            if (ModelState.IsValid)
            {
                var restTagsList = new List<Tag>();
                foreach(var tagcare in tagToCare)
                {
                    restTagsList.Add(_context.Tag.FirstOrDefault(m => m.Id == tagcare));
                }
                restaurant.Tags = restTagsList;
                _context.Add(address);
                await _context.SaveChangesAsync();
                restaurant.AddressId = address.Id;
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "City", restaurant.AddressId);
            return View(restaurant);
        }

        // Take Care the image Upload and returns the path
        private string ImageUpload(IFormFile inputImage)
        {
            // Create the image name (uses the time in order to avoid people overide pics)
            var pathToSaveInRest = DateTime.Now.Ticks.ToString() + Path.GetExtension(inputImage.FileName);
            var path = Path.Combine(restPicDir, pathToSaveInRest);

            // save the image in the server side
            using (var stream = new FileStream(path, FileMode.Create))
            {
                inputImage.CopyToAsync(stream);
            }
            
            return pathToSaveInRest;
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "City", restaurant.AddressId);
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AddressId,AveragePrice,PicturePath,Rate,About")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            ViewData["AddressId"] = new SelectList(_context.Address, "Id", "City", restaurant.AddressId);
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.Id == id);
        }
    }
}
