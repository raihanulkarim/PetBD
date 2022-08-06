using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetBD.Data;
using PetBD.Helpers;
using PetBD.Models;
using PetBD.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PetBD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeView = new HomeViewModels();
            homeView.Products = await context.Products.Where(r => r.IsActive == true && r.IsDelete == false).Include(r=> r.Category).ToListAsync();
            homeView.Sliders = await context.Sliders.Where(r=> r.IsActive == true).ToListAsync();
            ViewBag.SliderCount = homeView.Sliders.Count();
            return View(homeView);
        }
        [Route("product")]
        public async Task<IActionResult> Products(string category)
        {
            IEnumerable<Product> res = await context.Products.Where(r => r.IsActive == true && r.IsDelete == false).Include(r => r.Category).ToListAsync();
            if (!String.IsNullOrEmpty(category))
            {
                if(category == "all")
                {
                    res = await context.Products.Where(r => r.IsActive == true && r.IsDelete == false).Include(r => r.Category).ToListAsync();

                }else if (category == "new")
                {
                    res = await context.Products.Where(r => r.IsActive == true && r.IsDelete == false && r.IsFeatured == true).Include(r => r.Category).ToListAsync();
                }
            }
            return View(res);
        }
        [Route("payment/success")]
        public IActionResult PaymentSuccess()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
