using Microsoft.AspNetCore.Identity;

namespace AgriEnergy_ST10204001_POE_Part_2.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        [PersonalData]
        public string ContactNumber { get; set; }

        public FarmerDetail? FarmerDetail { get; set; }

        public Product? Product { get; set; }
    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
