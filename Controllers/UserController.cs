using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using onlineshopping.Data;
using onlineshopping.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShopping.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly onlineshoppingContext _context;

        public UserController(onlineshoppingContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [AllowAnonymous]
        // GET: User/Register 
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]

        // POST: User/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Email,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                var validUser = (from u in _context.User.ToList() where u.Email == user.Email select u).ToList();
                if (validUser.Count > 0)
                {
                    ViewData["Message"] = "Email is already exist";

                }
                else
                {
                    user.UserType = "normal";
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }
        [AllowAnonymous]
        // GET: User/Register
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]

        // POST: User/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var validUser = _context.User.ToList().Where(u => u.Email == user.Email && u.Password == user.Password).ToList();
                    //(from u in _context.User.ToList() where u.Email == user.Email select u).ToList();
                if (validUser.Count > 0)
                {
                    var claims = new List<Claim>();
                    var claim = new Claim(ClaimTypes.Email, user.Email);
                    var claimRole = new Claim(ClaimTypes.Role, validUser[0].UserType);
                    claims.Add(claim);
                    claims.Add(claimRole);
                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimPrinciple = new ClaimsPrincipal(claimIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrinciple);
                    return RedirectToAction("ProductList","Product");
                }
                else
                {
                    user.UserType = "normal";
                    _context.Add(user);
                    ViewData["Message"] = "Username or password is incorrect";
                }
            }
            return View(user);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ProductList", "Product");
           // return View();
        }
        // GET: User/Edit/5
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

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,UserType")] User user)
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

        // GET: User/Delete/5
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

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
