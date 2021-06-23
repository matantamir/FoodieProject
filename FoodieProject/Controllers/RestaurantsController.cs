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
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace FoodieProject.Controllers
{
    //[Authorize] - in order to make the all function accessble just for authnticated users
    //[Authorize(Roles ="Admin")] - in order to restrict function just for admins
    public class RestaurantsController : Controller
    {
        // Create a Directory in order to save pictures in servere side
        private readonly string restPicDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pictures\\Rest");

        private readonly FoodieProjectContext _context;

        public RestaurantsController(FoodieProjectContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            // Send the pic path to the view
            ViewData["picPath"] = "\\Pictures\\Rest\\";

            var foodieProjectContext = _context.Restaurant.Include(r => r.Address);
            ViewData["Tags"] = _context.Tag.ToList();

            return View(await foodieProjectContext.ToListAsync());
        }

        // GET: AveragePrice
        public async Task<IActionResult> AveragePrice()
        {

            var AVGmodel = _context.Dish.Include(r => r.Restaurant).GroupBy(d => d.Restaurant.Name).Select(a => new List<string> { a.Key.ToString(), a.Average(x => x.Price).ToString() });
            //var restListObj = _context.Restaurant.ToList();
            //ViewData["RestList"] = restListObj;

            ViewData["AVGmodel"] = AVGmodel;
            return View();
        }
        // GET: Articles
        public async Task<IActionResult> Articles()
        {
            
            WebRequest request = WebRequest.Create("https://api.twitter.com/2/users/110365072/tweets?max_results=5");
            request.Headers.Add("Authorization", "Bearer AAAAAAAAAAAAAAAAAAAAAJl8QwEAAAAAqc68K%2BOX9n%2BrIl9tOWiTtnsr0Kw%3DIJPft7t2lDWfEQmkryTNDr6X2X4pAvRK9HKn6tFIz5aNNquClX");
            using (System.IO.Stream s = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    ViewData["Tweets"] = jsonResponse;
                }
            }
            return View();
        }
        



        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.Address).Include(r => r.Tags).Include(r => r.Dishes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            // Send the pic path to the view
            ViewData["picPath"] = "\\Pictures\\Rest\\";

            return View(restaurant);
        }

        // GET: Restaurants/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //OLD-TAGS - ViewData["Tags"] = new MultiSelectList(_context.Tag, "Id", "Name");
            ViewData["Tags"] = _context.Tag.ToList();
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Phone,AddressId,AveragePrice,PicturePath,Rate,About")] Restaurant restaurant,
            [Bind("Id,City,Street,Number,PlaceId")] Address address,
            int[] Tags,
            // Get also a picture from user
            IFormFile myFile
            )
        {
            // Check if There is a restaurnts with such a name
            var restExists = _context.Restaurant.FirstOrDefault(r => r.Name == restaurant.Name);
   

            if (restExists == null)
            {
                // If we have got an image
                if (myFile != null)
                {
                    restaurant.PicturePath = ImageUpload(myFile);
                }

                var tags = _context.Tag.Where(t => Tags.Contains(t.Id));
                restaurant.Tags = new List<Tag>();
                restaurant.Tags.AddRange(tags);

                if (ModelState.IsValid)
                {

                    /*OLD- TAGS:
                     var restTagsList = new List<Tag>();
                     foreach(var tagcare in tagToCare)
                     {
                         restTagsList.Add(_context.Tag.FirstOrDefault(m => m.Id == tagcare));
                     }
                     restaurant.Tags = restTagsList;*/
                    _context.Add(address);
                    await _context.SaveChangesAsync();
                    restaurant.AddressId = address.Id;
                    _context.Add(restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                ViewData["ErrorRestExists"] = "Restaurant name already exists, Choose a different restaurant name";
            }

            //ViewData["AddressId"] = new SelectList(_context.Address, "Id", "City", restaurant.AddressId);
            ViewData["Tags"] = _context.Tag.ToList();
            ViewData["Rate"] = restaurant.Rate;
            ViewData["AveragePrice"] = restaurant.AveragePrice;
            //ViewData["restTags"] = restaurant.Tags.ToArray().ToString();
            
            var tempTags = _context.Tag.Where(t => Tags.Contains(t.Id));
            restaurant.Tags = new List<Tag>();
            restaurant.Tags.AddRange(tempTags);
            ViewData["restTags"] = restaurant.Tags;
            ViewData["serverError"] = "true";

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

                inputImage.CopyTo(stream);
            }

            return pathToSaveInRest;
        }

        // GET: Restaurants/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.Include(t => t.Tags).Where(r => r.Id == id).FirstOrDefaultAsync();
            if (restaurant == null)
            {
                return NotFound();
            }


            ViewData["Tags"] = _context.Tag.ToList();
            ViewData["Rate"] = restaurant.Rate;
            ViewData["AveragePrice"] = restaurant.AveragePrice;
            // var address = await _context.Address.Where(a => a.Id == restaurant.AddressId).FirstOrDefaultAsync();
            var address = await _context.Address.FindAsync(restaurant.AddressId);
            //ViewData["Street"] = address.Street;
            // ViewData["City"] = address.City;
            // ViewData["Number"] = address.Number;

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Phone,AddressId,AveragePrice,PicturePath,Rate,About")] Restaurant restaurant,
            [Bind("Id,City,Street,Number,PlaceId")] Address address,
            int[] Tags,
            IFormFile newPic)
        {
            // Check if we have got a new file. If we have got a new one we should delete the previous one and update to the new one
            if (newPic != null)
            {
                var oldRest = await _context.Restaurant.FirstOrDefaultAsync(m => m.Id == id);
                var oldPath = restPicDir + "\\" + oldRest.PicturePath;
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                oldRest.PicturePath = ImageUpload(newPic);
                _context.Update(oldRest);
                await _context.SaveChangesAsync();
            }

            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var rest = await _context.Restaurant.Include(t => t.Tags).Where(r => r.Id == id).FirstOrDefaultAsync();
                    var tags = _context.Tag.Where(t => Tags.Contains(t.Id));
                    // var addr2 = _context.Address.Where(a => a.Id == address.Id);
                    var addr = await _context.Address.FindAsync(rest.AddressId);
                    addr.Street = address.Street;
                    addr.City = address.City;
                    addr.Number = address.Number;
                    addr.PlaceId = address.PlaceId;

                    rest.Tags.Clear();
                    rest.Tags.AddRange(tags);

                    
                    rest.Name = restaurant.Name;
                    rest.AveragePrice = restaurant.AveragePrice;
                    rest.Phone = restaurant.Phone;
                    rest.Rate = restaurant.Rate;
                    rest.About = restaurant.About;

                    _context.Update(addr);
                    _context.Update(rest);
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> Search(string qAddr, string qRest, int qRate, int qPrice, int[] qTags)
        {
            {
                ViewData["picPath"] = "\\Pictures\\Rest\\";
                var results = _context.Restaurant.Include(t => t.Tags).Include(a => a.Address).Where(z => z.Name != null && z.Address.City != null && z.Tags.Count != null && z.Rate != null);

                if (qAddr != null)
                {
                    results = results.Where(r => r.Address.City.Contains(qAddr) || r.Address.Street.Contains(qAddr));
                }

                if (qRest != null)
                {
                    results = results.Where(r => r.Name.Contains(qRest)).Include(t => t.Tags);
                }


                if (qTags.Length != 0)
                {
                    foreach (var tag in qTags)
                        {
                        var tagObj = _context.Tag.Where(t => t.Id == tag).First();
                        results = results.Where(r => r.Tags.Contains(tagObj));
                        }
                    
                }

                if (qRate != 0)
                {
                    results = results.Where(r => r.Rate >= qRate);
                }

                if (qPrice != 0)
                {
                    results = results.Where(r => r.AveragePrice == qPrice);
                }

                var resRest = results.Select(z => new
                {
                    restId = z.Id,
                    restRate = z.Rate,
                    restCity = z.Address.City,
                    restStreet = z.Address.Street,
                    restAddrNum = z.Address.Number,
                    restPicPath = z.PicturePath,
                    restName = z.Name
                });

                //if (qDish != null)
                //{
                //    results = results.Where(d => d.Dishes.ForEach))
                //}


                //if (!RestaurantExists(restaurant.Id))
                //{
                //    return NotFound();
                //}


                return Json(await resRest.ToListAsync());

            }

        }
    }
}
