using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodieProject.Data;
using FoodieProject.Models;

namespace FoodieProject.Controllers
{
    public class DishTagsController : Controller
    {
        private readonly FoodieProjectContext _context;

        public DishTagsController(FoodieProjectContext context)
        {
            _context = context;
        }

        // GET: DishTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.DishTag.ToListAsync());
        }

        // GET: DishTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishTag == null)
            {
                return NotFound();
            }

            return View(dishTag);
        }

        // GET: DishTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DishTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DishTag dishTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishTag);
        }

        // GET: DishTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTag.FindAsync(id);
            if (dishTag == null)
            {
                return NotFound();
            }
            return View(dishTag);
        }

        // POST: DishTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DishTag dishTag)
        {
            if (id != dishTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishTagExists(dishTag.Id))
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
            return View(dishTag);
        }

        // GET: DishTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishTag = await _context.DishTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishTag == null)
            {
                return NotFound();
            }

            return View(dishTag);
        }

        // POST: DishTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishTag = await _context.DishTag.FindAsync(id);
            _context.DishTag.Remove(dishTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishTagExists(int id)
        {
            return _context.DishTag.Any(e => e.Id == id);
        }
    }
}
