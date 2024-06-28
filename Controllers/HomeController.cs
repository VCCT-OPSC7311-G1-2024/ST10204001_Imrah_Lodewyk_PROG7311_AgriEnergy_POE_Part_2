using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AgriEnergy_ST10204001_POE_Part_2.Controllers
{
	[Authorize]
	public class HomeController : Controller
    {
        //Declaration of Variables
        private readonly ILogger<HomeController> _logger;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Index Interface
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// Sustainable Farming Hub
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Hub()
		{
			return View();
		}
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// Green Energy MarketPlace interface
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult MarketPlace()
		{
			return View();
		}
	}
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//