using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.TrainerViewModels
{
    public class TrainersViewModel
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Specialization { get; set; } = null!;

        #region Get Trainer Details 
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }

        #endregion

    }
}
