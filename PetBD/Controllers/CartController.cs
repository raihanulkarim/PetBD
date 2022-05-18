using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetBD.Data;
using PetBD.Helpers;
using PetBD.Models;
using PetBD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBD.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<Item>>("cart");
            ViewBag.CartCounter = 0;
            ViewBag.cart = cart;
            if (cart != null)
            {
                ViewBag.total = cart.Sum(r => r.Product.Price * r.Quantity);
                ViewBag.CartCounter = cart.Count();
            }
            return View();
        }
        public IActionResult AddtoCart(int id)
        {
            if (HttpContext.Session.Get<List<Item>>("cart") == null)
            {

                var cart = new List<Item>();
                var prod = context.Products.Find(id);
                cart.Add(new Item()
                {
                    Product = prod,
                    Quantity = 1
                });
                HttpContext.Session.Set("cart", cart);
            }
            else
            {
                var cart = HttpContext.Session.Get<List<Item>>("cart");
                var prod = context.Products.Find(id);
                cart.Add(new Item()
                {
                    Product = prod,
                    Quantity = 1
                });
                HttpContext.Session.Set("cart", cart);
            }
            return RedirectToAction("index","home");
        }
        public IActionResult UpdateCart(IFormCollection fc)
        {
            string[] quntities = fc["Qty"];
            List<Item> cart = HttpContext.Session.Get<List<Item>>("cart");
            for (int i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = Convert.ToInt32(quntities[i]);
            }

            HttpContext.Session.Set("cart", cart);
            return RedirectToAction("index");
        }
      public IActionResult Remove(int id)
        {
            List<Item> cart = HttpContext.Session.Get<List<Item>>("cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            HttpContext.Session.Set("cart", cart);
            return RedirectToAction("index");

        }
        private int isExist(int id)
        {
            List<Item> cart = HttpContext.Session.Get<List<Item>>("cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
