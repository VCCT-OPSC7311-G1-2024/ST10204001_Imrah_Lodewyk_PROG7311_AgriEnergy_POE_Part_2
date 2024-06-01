using AgriEnergy_ST10204001_POE_Part_2.Data;
using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Syncfusion.EJ2.Linq;

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
		/// <summary>
		/// HttpGet method for viewing list Products Interface 
		/// </summary>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="category"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Index(DateTime? startDate, DateTime? endDate, string category)
		{
			// Get the current user's UserId
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Query to get products for the logged-in user
			var products = _context.Products
								   .Where(p => p.UserId == userId)
								   .AsQueryable();

			// Apply date range filter
			if (startDate.HasValue)
			{
				products = products.Where(p => p.ProductionDate >= startDate.Value);
			}

			if (endDate.HasValue)
			{
				products = products.Where(p => p.ProductionDate <= endDate.Value);
			}

			// Apply category filter
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
		/// <summary>
		/// HttpGet method for Adding Product Interface 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public IActionResult Add()
        {
            return View();
        }
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// HttpGet method for Adding Product Interface 
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
			// Retrieve the current user's UserId
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Retrieve the FarmerId associated with the current user
			var farmer = await _context.FarmerDetails
				.FirstOrDefaultAsync(f => f.UserId == userId);

			if (farmer == null)
			{
				// Handle the case where no farmer is found for the user
				ModelState.AddModelError("", "No farmer details found for the current user.");
				return View(product); // Return the view with the product to show errors
			}

			var newProduct = new Product()
			{
				ProductId = Guid.NewGuid(),
				FarmerId = farmer.FarmerDetailId,
				UserId = userId,
				ProductName = product.ProductName,
				Category = product.Category,
				ProductionDate = product.ProductionDate,
			};

			await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();  

            return RedirectToAction("Add");
        }
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// HttpGet method for Educational and Training Resources View
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Resources()
		{
			return View();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// HttpGet method for Project Collaboration and Funding Opportunities View
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Collab()
		{
			return View();
		}
	}
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//