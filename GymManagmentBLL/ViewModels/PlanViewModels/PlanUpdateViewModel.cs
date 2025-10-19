using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.PlanViewModels
{
    public class PlanUpdateViewModel
    {
        public string PlanName { get; set; } = null!;


        [Required(ErrorMessage = "Plan Description is required.")]
        [StringLength(200, ErrorMessage = "Plan Description cannot exceed 200 characters.")]
        public string Description { get; set; } = null!;


        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10000.00.")]
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }


        [Range(1, 365, ErrorMessage = "DurationDays must be between 1 and 365.")]
        [Required(ErrorMessage = "DurationDays is required.")]
        public int DurationDays { get; set; }

    }
}
