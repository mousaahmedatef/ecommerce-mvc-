using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Models
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage ="Password is required")]
        [MinLength(5,ErrorMessage ="minimum Password Length is 5")]  
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "confirm password does not match  password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
