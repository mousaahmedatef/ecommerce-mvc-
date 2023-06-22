using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        //[RegularExpression("[a-zA-Z]{2, 4}[0,9]{2, 3}$", ErrorMessage = "Code should be like HR11")]
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }= DateTime.Now;

        public virtual ICollection<Employee> MyProperty { set; get; } = new HashSet<Employee>();
    }
}
