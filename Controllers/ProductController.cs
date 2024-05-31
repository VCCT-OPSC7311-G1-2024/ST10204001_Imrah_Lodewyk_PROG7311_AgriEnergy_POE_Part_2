using AgriEnergy_ST10204001_POE_Part_2.Data;
using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AgriEnergy_ST10204001_POE_Part_2.Controllers
{
	[Authorize(Roles = "Farmer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="context"></param>
        public ProductController(ApplicationDbContext context)
        {
            this._context = context;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		[HttpGet]
		public IActionResult Index(DateTime? startDate, DateTime? endDate, string category)
		{
			var products = _context.Products.AsQueryable();

			if (startDate.HasValue)
			{
				products = products.Where(p => p.ProductionDate >= startDate.Value);
			}

			if (endDate.HasValue)
			{
				products = products.Where(p => p.ProductionDate <= endDate.Value);
			}

			if (!string.IsNullOrEmpty(category))
			{
				products = products.Where(p => p.Category == category);
			}

			// Get unique categories
			var categories = _context.Products
									 .Select(p => p.Category)
									 .Distinct()
									 .ToList();

			// Create a list of SelectListItem objects
			var categoryItems = categories.Select(c => new SelectListItem
			{
				Text = c,
				Value = c,
				Selected = c == category // Set selected item based on current category
			}).ToList();

			// Pass the filter values and categories to the view
			ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
			ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
			ViewBag.Category = category;
			ViewBag.Categories = categoryItems;


			return View(products.ToList());
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		[HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            // Retrieve the current user's UserId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newProduct = new Product() 
            {
                ProductId = Guid.NewGuid(),
				UserId = userId,
                ProductName = product.ProductName,
                Category = product.Category,
                ProductionDate = product.ProductionDate,
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();  

            return RedirectToAction("Add");
        }

    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//