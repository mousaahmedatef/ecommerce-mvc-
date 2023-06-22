using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Models
{
    public class UserInRoleVM
    {
        public string UserId { get; set; }
        public bool IsSelected { get; set; }
        public string UserName { get; set; }
    }
}
