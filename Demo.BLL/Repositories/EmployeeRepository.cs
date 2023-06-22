using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(MVCAppDbContext context) : base(context)
        {

        }

        public IEnumerable<Employee> GetEmployeeByDepartmentName(string departmentName)
        {
            throw new NotImplementedException();
        }
    }
}
