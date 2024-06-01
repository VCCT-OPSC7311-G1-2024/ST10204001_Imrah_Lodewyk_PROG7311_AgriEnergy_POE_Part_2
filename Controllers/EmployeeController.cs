using AgriEnergy_ST10204001_POE_Part_2.Areas.Identity.Pages.Account;
using AgriEnergy_ST10204001_POE_Part_2.Data;
using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace AgriEnergy_ST10204001_POE_Part_2.Controllers
{
    //Enable Authorisation for only employees
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        //Declaration of variables
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public EmployeeController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// HTTPGET method for adding farmers 
        /// </summary>
        /// <returns>Adding farmers view</returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// HTTPPost method for adding farmers 
		/// </summary>
		/// <param name="email"></param>
		/// <param name="firstName"></param>
		/// <param name="surname"></param>
		/// <param name="contactNumber"></param>
		/// <param name="address"></param>
		/// <param name="farmType"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> Add(string email, string firstName, string surname, string contactNumber, string address, string farmType, User user)
        {
            address = user.FarmerDetail.Address;
            farmType = user.FarmerDetail.FarmType;

            try
            {
                var userModel = new User
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    Surname = surname,
                    ContactNumber = contactNumber
                };

                //Add farmer with temporary password 
                var result = await _userManager.CreateAsync(userModel, "TemporaryPassword@1");

                if (result.Succeeded)
                {
                    // Assign the user to the role
                    await _userManager.AddToRoleAsync(userModel, "Farmer");

                    var farmerDetailModel = new FarmerDetail
                    {
                        FarmerDetailId = Guid.NewGuid(),
                        UserId = userModel.Id,
                        Address = address,
                        FarmType = farmType
                    };

                    _context.FarmerDetails.Add(farmerDetailModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            }

            return View();
        }

		/* IQueryable<Product> products = _context.Products.Include(f => f.UserModel);

	 if (!string.IsNullOrEmpty(category))
	   {
		   // Join FarmerModel with ProductModel based on UserID
		   products = products.Join(_context.Products,
								   farmer => farmer.UserId,
								   product => product.UserId,
								   (farmer, product) => new { Farmer = farmer, Product = product })
							.Where(fp => fp.Product.Category == category)
							.Select(fp => fp.Farmer)
							.Distinct();
	   }

	   if (!string.IsNullOrEmpty(farmType))
	   {
		   products = products.Where(f => f.FarmerModel.FarmType == farmType);
	   }

	   // Get unique categories
	   var categories = _context.Products
						.Select(p => p.Category)
						.Distinct()
						.ToList();

	   // Create a list of SelectListItem objects for categories
	   var categoryItems = categories.Select(c => new SelectListItem
	   {
		   Text = c,
		   Value = c,
		   Selected = c == category // Set selected item based on current category
	   }).ToList();

	   // Get unique farm types
	   var farmTypes = _context.FarmerDetails
							.Select(f => f.FarmType)
							.Distinct()
							.ToList();

	   // Create a list of SelectListItem objects for farm types
	   var farmTypeItems = farmTypes.Select(ft => new SelectListItem
	   {
		   Text = ft,
		   Value = ft,
		   Selected = ft == farmType // Set selected item based on current farm type
	   }).ToList();


	   if (startDate.HasValue)
	   {
		   products = products.Where(p => p.ProductionDate >= startDate.Value);
	   }

	   if (endDate.HasValue)
	   {
		   products = products.Where(p => p.ProductionDate <= endDate.Value);
	   }

	   // Pass the filter values and dropdown items to the view
	   ViewBag.Category = category;
	   ViewBag.Categories = categoryItems;
	   ViewBag.FarmType = farmType;
	   ViewBag.FarmTypes = farmTypeItems;

	   ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
	   ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");*/

		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// HTTPGET method for list of all farmers
		/// </summary>
		/// <param name="category"></param>
		/// <param name="farmType"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		[HttpGet]
        public async Task<IActionResult> Index(string category, string farmType, DateTime? startDate, DateTime? endDate)
        {

			// Query to get products for the logged-in user
			IQueryable<Product> products = _context.Products
												   .Include(p => p.UserModel)
												   .Include(p => p.FarmerModel);

			// Apply filters
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

			if (!string.IsNullOrEmpty(farmType))
			{
				products = products.Where(p => p.FarmerModel.FarmType == farmType);
			}

			// Get unique categories for the current user's products
			var categories = await _context.Products

										   .Select(p => p.Category)
										   .Distinct()
										   .ToListAsync();

			// Create a list of SelectListItem objects for categories
			var categoryItems = categories.Select(c => new SelectListItem
			{
				Text = c,
				Value = c,
				Selected = c == category // Set selected item based on current category
			}).ToList();

			// Get unique farm types for the current user's products
			var farmTypes = await _context.FarmerDetails
										  .Select(f => f.FarmType)
										  .Distinct()
										  .ToListAsync();

			// Create a list of SelectListItem objects for farm types
			var farmTypeItems = farmTypes.Select(ft => new SelectListItem
			{
				Text = ft,
				Value = ft,
				Selected = ft == farmType // Set selected item based on current farm type
			}).ToList();

			// Pass the filter values and categories to the view
			ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
			ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
			ViewBag.Category = category;
			ViewBag.Categories = categoryItems;
			ViewBag.FarmType = farmType;
			ViewBag.FarmTypes = farmTypeItems;

			var model = await products.ToListAsync();

            return View(model);

        }

    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//