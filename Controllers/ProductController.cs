using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using onlineshopping.Data;
using onlineshopping.Models;

namespace onlineshopping.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly onlineshoppingContext _context;

        public ProductController(onlineshoppingContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var onlineshoppingContext = _context.Product.Include(p => p.Category);
            return View(await onlineshoppingContext.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> Productlist()
        {
            var onlineshoppingContext = _context.Product.Include(p => p.Category);
            return View(await onlineshoppingContext.ToListAsync());
        }
        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["categoryid"] = new SelectList(_context.Category, "id", "name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,description,product_icon,categoryid")] Product product,IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                Guid guid = Guid.NewGuid();
                string fileEx = photo.FileName.Substring(photo.FileName.IndexOf('.'));
                string path = Environment.CurrentDirectory + "/wwwroot/Images/" + guid + fileEx;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(fileStream);
                product.product_icon = guid + fileEx;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoryid"] = new SelectList(_context.Category, "id", "id", product.categoryid);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["categoryid"] = new SelectList(_context.Category, "id", "id", product.categoryid);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,description,product_icon,categoryid")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["categoryid"] = new SelectList(_context.Category, "id", "id", product.categoryid);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
