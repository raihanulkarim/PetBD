using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetBD.Data;
using PetBD.Helpers;
using PetBD.Models;
using Stripe;

namespace PetBD.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<Item>>("cart");
            ViewBag.CartCounter = 0;
            ViewBag.cart = cart;
            ViewBag.shipping = 50;
            if (cart != null)
            {
                ViewBag.subTotal = cart.Sum(r => r.Product.Price * r.Quantity);
                ViewBag.total = cart.Sum(r => r.Product.Price * r.Quantity) + ViewBag.shipping;
                ViewBag.CartCounter = cart.Count();
            }
            return View();
        }

        public async Task<IActionResult> Payment(string stripeEmail, string stripeToken, Models.Order order)
        {
            var cart = HttpContext.Session.Get<List<Item>>("cart");
            int CartCounter = 0;
            int shippingCost = 50;
            int total = 0;
            if (cart != null)
            {
                int subTotal = cart.Sum(r => r.Product.Price * r.Quantity);
                total = cart.Sum(r => r.Product.Price * r.Quantity) + shippingCost;
                CartCounter = cart.Count();
            }
            var charges = new ChargeService();
            var service = new CustomerService();
            var stripeCustomers = await service.ListAsync(new CustomerListOptions()
            {
                Email = stripeEmail
            });
            var customer = new Customer();
            if (stripeCustomers.Any())
            {
                customer = stripeCustomers.Single();
            }
            else
            {
                customer = service.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });
            }
            var rand = new Random(10);
            var orderNo = rand.Next().ToString();
            var orders = await _context.Order.ToListAsync();
            while (orders.Any(r=> r.OrderNo == orderNo))
            {
                orderNo = rand.Next().ToString();
            }
            order.OrderNo = orderNo;
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = total * 100,
                Description = CartCounter + "Products",
                Currency = "bdt",
                Customer = customer.Id,
                ReceiptEmail = stripeEmail,
                Metadata = new Dictionary<string, string>() {
                    {"OrderId", order.OrderNo },
                    {"PostCode", order.ZipCode },
                    {"Customer Name", order.FirstName+" "+order.LastName},
                    {"Address", order.Address },
                    {"PhoneNo", order.PhoneNo },
                }
            });
            if (charge.Status == "succeeded")
            {
                string BalanceTnx = charge.BalanceTransactionId;
                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");
                return RedirectToAction("PaymentSuccess", "home");
            }
            else
            {
                return View();
            }

        }

    }
}
