using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy_ST10204001_POE_Part_2.Models
{
    public class FarmerDetail
    {
		[Key]
		public Guid FarmerDetailId { get; set; }

		//--------------Foreign keys-----------------
		[ForeignKey("UserModel")]
		public string UserId { get; set; } 
		public User UserModel { get; set; }

		//--------------Foreign keys-----------------

		[PersonalData]
		public string Address { get; set; }

		[PersonalData]
		public string FarmType { get; set; }

		public ICollection<Product> Products { get; set; }
	}
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//