using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Demo.BLL.Models;
using Demo.DAL.Entities;

namespace Demo.BLL.Mapper
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<EmployeeVM, Employee>();
            CreateMap<Employee,EmployeeVM>();

            CreateMap<DepartmentVM, Department>();
            CreateMap<Department, DepartmentVM>();
        }
    }
}
