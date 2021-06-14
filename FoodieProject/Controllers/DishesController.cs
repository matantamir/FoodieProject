﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodieProject.Data;
using FoodieProject.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FoodieProject.Controllers
{
    public class DishesController : Controller
    {
        // Create the relative path for the dishes pictures folder
        private readonly string dishPicDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Pictures\\Dish");

        private readonly FoodieProjectContext _context;

        public DishesController(FoodieProjectContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var dishesListWithRest = _context.Dish.Include(d => d.Restaurant);
            return View(await dishesListWithRest.ToListAsync());
        }

        public async Task<IActionResult> Search(string query)
        {
            var dishSearch = _context.Dish.Include(d => d.Restaurant).Where(d => (d.Name.Contains(query) || d.Description.Contains(query)) || query == null).Select(z => new
            { 
                name = z.Name,
                description = z.Description,
                price = z.Price,
                picturePath = z.PicturePath,
                restName = z.Restaurant.Name
            });
            return Json(await dishSearch.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            ViewData["picPath"] = "\\Pictures\\Dish\\";

            return View(dish);
        }

        // GET: Dishes/Create
        public async Task<IActionResult> Create(int Restid)
        {
            if (Restid == 0)
            {
                ViewData["RestaurantId"] = new SelectList(_context.Set<Restaurant>(), "Id", "Name");
            }
            else
            {
                var restaurant = _context.Restaurant.FirstOrDefault(m => m.Id == Restid);

                if (restaurant == null)
                {
                    return NotFound();
                }

                ViewData["dishRestName"] = restaurant.Name;
                ViewData["dishRestId"] = restaurant.Id;
            }
            //ViewData["RestaurantId"] = new SelectList(_context.Set<Restaurant>(), "Id", "Name");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,PicturePath,RestaurantId")] Dish dish, IFormFile myFile)
        {
            // If we have got an image
            if (myFile != null)
            {
                // Save the path in the resturant object
                dish.PicturePath = ImageUpload(myFile);
            }

            var restaurant = _context.Restaurant.FirstOrDefault(m => m.Id == dish.RestaurantId);
            dish.Restaurant = restaurant;
            dish.RestaurantId = restaurant.Id;
            if (ModelState.IsValid)
            {
               // var restaurant = _context.Restaurant.FirstOrDefault(m => m.Id == 1);
                //dish.Restaurant = restaurant;
              //  dish.RestaurantId = restaurant.Id;
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // Take Care the image Upload and returns the path
        private string ImageUpload(IFormFile inputImage)
        {
            // Create the image name (uses the time in order to avoid people overide pics)
            var pathToSaveInRest = DateTime.Now.Ticks.ToString() + Path.GetExtension(inputImage.FileName);
            var path = Path.Combine(dishPicDir, pathToSaveInRest);

            // save the image in the server side
            using (var stream = new FileStream(path, FileMode.Create))
            {
                inputImage.CopyTo(stream);
            }

            return pathToSaveInRest;
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            var restaurant = _context.Restaurant.FirstOrDefault(m => m.Id == dish.RestaurantId);
            ViewData["dishRestName"] = restaurant.Name;

            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,PicturePath,RestaurantId")] Dish dish,
                    IFormFile newPic)
        {
          
            if (id != dish.Id)
            {
                return NotFound();
            }

            var oldDish = await _context.Dish.FirstOrDefaultAsync(m => m.Id == id);

            // Check if we have got a new file. If we have got a new one we should delete the previous one and update to the new one
            if (newPic != null)
            {

                var oldPath = dishPicDir + "\\" + oldDish.PicturePath;
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                oldDish.PicturePath = ImageUpload(newPic);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    oldDish.Name = dish.Name;
                    oldDish.Description = dish.Description;
                    oldDish.Price = dish.Price;
                    oldDish.RestaurantId = dish.RestaurantId;
                    _context.Update(oldDish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}
