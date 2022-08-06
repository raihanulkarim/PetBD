using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetBD.Data;
using PetBD.Models;

namespace PetBD.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive,CreatedDate,ModifiedDate,Description,IsFeatured,Quantity,Price,Image,CatID,UploadImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                //checking whether category has been selected or not, if selected then assign the relevent category to the product
                if(product.CatID != 0)
                {
                    product.Category = await _context.Categories.FindAsync(product.CatID);

                }
                product.Image = await PhotoUpload(product);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive,CreatedDate,ModifiedDate,Description,IsFeatured,Quantity,Price,Image,CatID,UploadImage")] Product product)
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
                    if (product.UploadImage != null)
                    {
                        if (prod.Image != null)
                        {
                            DeletePhoto(prod.Image);
                        }
                        prod.Image = await PhotoUpload(product);
                    }
                    prod.IsFeatured = product.IsFeatured;
                    prod.Name = product.Name;
                    prod.Description = product.Description;
                    prod.Quantity = product.Quantity;
                    prod.Price = product.Price;
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
            DeletePhoto(product.Image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<string> PhotoUpload(Product product)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(product.UploadImage.FileName);
            string extenstion = Path.GetExtension(product.UploadImage.FileName);
            fileName = fileName = "product-" + fileName + DateTime.Now.ToString("yymmssfff") + extenstion;
            string path = Path.Combine(wwwRootPath + "/assets/img/products/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await product.UploadImage.CopyToAsync(fileStream);
            }
            return fileName;
        }
        private void DeletePhoto(string imgName)
        {
            var imgPath = Path.Combine(webHostEnvironment.WebRootPath + "/assets/img/products", imgName);
            FileInfo f = new FileInfo(imgName);
            if (f != null)
            {
                System.IO.File.Delete(imgPath);
                f.Delete();
            }
        }
    }
}
