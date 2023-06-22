using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Invaild Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(5,ErrorMessage = "Minimum Password Length is 5")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage ="Confirm Password does not match Password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
