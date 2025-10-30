using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.ViewModels.AccountViewModels
{
    public class AccountViewModel
    {
        [Required(ErrorMessage ="Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;



        [Required(ErrorMessage ="Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;



        public bool RememberMe { get; set; }
    }
}
