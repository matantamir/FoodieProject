using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodieProject.Data;
using FoodieProject.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FoodieProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly FoodieProjectContext _context;

        public UsersController(FoodieProjectContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string query)
        {
            var userSearch = _context.User.Where(u => u.Username.Contains(query) || query == null);
            return Json(await userSearch.ToListAsync());
        }

        //GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var userExists = _context.User.FirstOrDefault(u => u.Username == user.Username);

                if (userExists == null)
                {
                    // User type is Client by default
                    user.Type = UserRole.Client;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    // Log the user in automaticaly after registration
                    var loginUser = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                    Signin(loginUser);

                    return RedirectToAction(nameof(Index), "Restaurants");
                }
                else
                {
                    ViewData["ErrorUserExists"] = "Username already exists, Choose a different username"; 
                }

                
            }
            return View(user);
        }

        //GET: Users/RegisterAdmin
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin([Bind("Id,Username,Password,Type")] User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var userExists = _context.User.FirstOrDefault(u => u.Username == user.Username);

                if (userExists == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), "Users");
                }
                else
                {
                    ViewData["ErrorUserExists"] = "Username already exists, Choose a different username";
                }


            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //GET: Users/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        //GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the username and the password are correct
                var userExists = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                if (userExists != null)
                {
                    // Add details to the session
                    Signin(userExists);
                    return RedirectToAction(nameof(Index), "Restaurants");
                }
                else
                {
                    ViewData["ErrorLoginFailed"] = "Username or password are incorrect, please try again";
                }


            }
            return View(user);
        }

        private async void Signin(User account)
        {
            // Save details about the user in the session
            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Type.ToString()),
            }; 
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties

            {
                // Save user session for 10 minutes
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
             };
            
            // Sign in function 
            await HttpContext.SignInAsync(

                // Cookie type authentication 
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                
                // Properties of authentication
                authProperties);
        }



        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        //        // GET: Users/Details/5
        //        public async Task<IActionResult> Details(int? id)
        //        {
        //            if (id == null)
        //            {
        //                return NotFound();
        //            }

        //            var user = await _context.User
        //                .FirstOrDefaultAsync(m => m.Id == id);
        //            if (user == null)
        //            {
        //                return NotFound();
        //            }

        //            return View(user);
        //        }

        //        // GET: Users/Create
        //        public IActionResult Create()
        //        {
        //            return View();
        //        }

        //        // POST: Users/Create
        //        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Create([Bind("Id,Username,Password,Type")] User user)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _context.Add(user);
        //                await _context.SaveChangesAsync();
        //                return RedirectToAction(nameof(Index));
        //            }
        //            return View(user);
        //        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Type")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
