using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergy_ST10204001_POE_Part_2.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        //--------------Foreign keys-----------------
        [ForeignKey("UserModel")]
        public string UserId { get; set; }
        public User UserModel { get; set; }

		[ForeignKey("FarmerModel")]
		public Guid FarmerId { get; set; }
		public FarmerDetail FarmerModel { get; set; }

		//--------------Foreign keys-----------------

		public string ProductName { get; set; }

        public string Category { get; set; }

        public DateTime? ProductionDate { get; set; }
	}
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------//