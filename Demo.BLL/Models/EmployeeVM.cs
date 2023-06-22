using Demo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Models
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required!")]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50!")]
        [MinLength(5, ErrorMessage = "Minimum Length Is 5!")]
        public string Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age must be between 22 and 60")]
        public int? Age { get; set; }

        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$"
            , ErrorMessage = "Address must be like '123-Street-City'")]
        public string Address { get; set; }
        [DataType(DataType.Currency)] //decimal بدل م يكون money بتاعو datatype دى عشان اخلى ال
        [Range(4000, 8000, ErrorMessage = "Salary Must be between 4000 and 8000")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int DepartmentId { get; set; }
        public virtual Department Department { set; get; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
    }
}
