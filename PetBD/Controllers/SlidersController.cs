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
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public SlidersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.OrderByDescending(r=> r.Id).ToListAsync());
        }

        // GET: Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SubTitle,Image,UploadImage,IsActive")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                slider.Image = await PhotoUpload(slider);
                _context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // GET: Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }
            var slid = await _context.Sliders.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if(slider.UploadImage != null)
                    {
                        if (slid.Image != null)
                        {
                            DeletePhoto(slid.Image);
                        }
                        slid.Image = await PhotoUpload(slider);
                    }
                    slid.SubTitle = slider.SubTitle;
                    slid.Title = slider.Title;
                    slid.IsActive = slider.IsActive;
                    _context.Update(slid);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
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
            return View(slid);
        }

        // GET: Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            DeletePhoto(slider.Image);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
        public async Task<string> PhotoUpload(Slider slider)
        {
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(slider.UploadImage.FileName);
            string extenstion = Path.GetExtension(slider.UploadImage.FileName);
            fileName = fileName = "Slider-" + fileName + DateTime.Now.ToString("yymmssfff") + extenstion;
            string path = Path.Combine(wwwRootPath + "/assets/img/slider/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.UploadImage.CopyToAsync(fileStream);
            }
            return fileName;
        }
        private void DeletePhoto(string imgName)
        {
            var imgPath = Path.Combine(webHostEnvironment.WebRootPath +"/assets/img/slider", imgName);
            FileInfo f = new FileInfo(imgName);
            if(f != null)
            {
                System.IO.File.Delete(imgPath);
                f.Delete();
            }
        }
    }
}
