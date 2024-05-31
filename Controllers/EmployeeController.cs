using AgriEnergy_ST10204001_POE_Part_2.Areas.Identity.Pages.Account;
using AgriEnergy_ST10204001_POE_Part_2.Data;
using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace AgriEnergy_ST10204001_POE_Part_2.Controllers
{

    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
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
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        [HttpGet]
        public async Task<IActionResult> Index(string category, string farmType)
        {

            IQueryable<FarmerDetail> farmers = _context.FarmerDetails.Include(f => f.UserModel);

            if (!string.IsNullOrEmpty(category))
            {
                // Join FarmerModel with ProductModel based on UserID
                farmers = farmers.Join(_context.Products,
                                        farmer => farmer.UserId,
                                        product => product.UserId,
                                        (farmer, product) => new { Farmer = farmer, Product = product })
                                 .Where(fp => fp.Product.Category == category)
                                 .Select(fp => fp.Farmer)
                                 .Distinct();
            }

            if (!string.IsNullOrEmpty(farmType))
            {
                farmers = farmers.Where(f => f.FarmType == farmType);
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

            // Pass the filter values and dropdown items to the view
            ViewBag.Category = category;
            ViewBag.Categories = categoryItems;
            ViewBag.FarmType = farmType;
            ViewBag.FarmTypes = farmTypeItems;

            var model = await farmers.ToListAsync();

            return View(model);

        }

    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//