using AgriEnergy_ST10204001_POE_Part_2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergy_ST10204001_POE_Part_2.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
		//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// DbSets of EFs
        /// </summary>
        public override DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FarmerDetail> FarmerDetails { get; set; }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        public async Task<bool> IsEmailInUseAsync(string email)
        {
            return await Users.AnyAsync(u => u.Email == email);
        }
    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//