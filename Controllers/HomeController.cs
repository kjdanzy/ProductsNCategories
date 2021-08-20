using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsNCategories.Models;

namespace ProductsNCategories.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Products");
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.ProductList = _context.Products
                .ToList();
                
            return View("Products");
        }

        [HttpPost("saveproduct")]
        public IActionResult SaveProduct(Product newProduct)
        {
            if (ModelState.IsValid){
                _context.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }

            return View("Products", newProduct);
        }

        [HttpGet("productpage/{productId}")]
        public IActionResult ProductsPage(int productId)
        {
            ViewBag.Product = _context.Products
                .Include(assoc => assoc.Associations)
                .ThenInclude(cat => cat.category)
                .FirstOrDefault(prod => prod.ProductId == productId);

            ViewBag.CategorySelect = _context.Categories
                .Include(assoc => assoc.Associations)
                .Where(c => c.Associations.All(a => a.ProductId != productId))
                .ToList();

            return View();
        }

        [HttpPost("addCategoryToProduct")]
        public IActionResult ACTP(Association newAsso)
        {
                _context.Add(newAsso);
                _context.SaveChanges();
                return Redirect($"/productpage/{newAsso.CategoryId}");
        }

        [HttpGet("categories")]
        public IActionResult DisplayCategories()
        {
            ViewBag.Categories = _context.Categories
                .ToList();
            return View("Categories");
        }

        [HttpPost("savecategory")]
        public IActionResult SaveCategory(Category newCategory)
        {
            if (ModelState.IsValid){
                _context.Add(newCategory);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View("Categories");
        }

        [HttpGet("categorypage/{categoryId}")]
        public IActionResult CategoryPage(int categoryId)
        {
            ViewBag.Category = _context.Categories
                .Include(assoc => assoc.Associations)
                .ThenInclude(prod => prod.product)
                .FirstOrDefault(cat => cat.CategoryId == categoryId);

            ViewBag.ProductSelect = _context.Products
                .Include(assoc => assoc.Associations)
                .Where(p => p.Associations.All(a => a.CategoryId != categoryId))
                .ToList();

            return View();
        }

        [HttpPost("addProductToCategory")]
        public IActionResult APTC(Association newAsso)
        {
                _context.Add(newAsso);
                _context.SaveChanges();
                return Redirect($"/productpage/{newAsso.ProductId}");
        }



        public IActionResult Privacy()
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
