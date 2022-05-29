using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetBD.Data;
using PetBD.Models;

namespace PetBD.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(r=> r.Category).Where(r=>r.IsDelete == false).OrderByDescending(r => r.Id).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(r => r.Category).Where(r => r.IsDelete == false)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //getting all the category to show into the view
            ViewData["categories"] = _context.Categories.Where(r => r.IsDelete == false).ToList();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,CreatedDate,ModifiedDate,Description,IsFeatured,Quantity,Image,CatID")] Product product)
        {
            if (ModelState.IsValid)
            {
                //checking whether category has been selected or not, if selected then assign the relevent category to the product
                if(product.CatID != 0)
                {
                    product.Category = await _context.Categories.FindAsync(product.CatID);

                }
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //retrieving non deleted products 
            var product = await _context.Products.Where(r => r.IsDelete == false).Include(r => r.Category).FirstOrDefaultAsync(r => r.Id == id);
            ViewData["categories"] = _context.Categories.Where(r => r.IsDelete == false).ToList();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,CreatedDate,ModifiedDate,Description,IsFeatured,Quantity,Image,CatID")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            var prod = await _context.Products.FindAsync(id);

            if (ModelState.IsValid)
            {
                try
                {
                    prod.IsFeatured = product.IsFeatured;
                    prod.Name = product.Name;
                    prod.Description = product.Description;
                    prod.Quantity = product.Quantity;
                    //prod.Image = product.Image;
                    prod.IsActive = product.IsActive;
                    prod.ModifiedDate = DateTime.Now;
                    //cheking if new category selected then update it
                    if (product.CatID != 0)
                    {
                        prod.Category = await _context.Categories.FindAsync(product.CatID);
                        prod.CatID = product.CatID;
                    }
                    _context.Update(prod);
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
            return View(prod);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Where(r => r.IsDelete == false)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            //this will not delete the product from the database rather it will update the 'isDelete' column of the Product entity
            product.IsDelete = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
