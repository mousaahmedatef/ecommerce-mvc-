using AutoMapper;
using Demo.BLL.Helper;
using Demo.BLL.Interfaces;
using Demo.BLL.Models;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.UnitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var employees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(await UnitOfWork.EmployeeRepository.GetAll());
                return View(employees);
            }
            else
            {
                var employees = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(await UnitOfWork.EmployeeRepository.SearchByName(SearchValue));
                return View(employees);
            }

                
           
        }
         
        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (id == null)
                return NotFound();

            var employee = await UnitOfWork.EmployeeRepository.Get(id);
            var EmployeeVM = mapper.Map<Employee, EmployeeVM>(employee);

            if (EmployeeVM == null)
                return NotFound();

            ViewBag.Departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(await UnitOfWork.DepartmentRepository.GetAll());
            return View(ViewName, EmployeeVM);
        }

        [HttpGet]
        public  async Task<IActionResult> Create()
        {
            ViewBag.Departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(await UnitOfWork.DepartmentRepository.GetAll());

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult>  Create(EmployeeVM EmployeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                EmployeeVM.ImageName =DocumentSetting.UploadFile(EmployeeVM.Image,"Imgs");
                var employee = mapper.Map<EmployeeVM, Employee>(EmployeeVM);
                await UnitOfWork.EmployeeRepository.Add(employee);
                return RedirectToAction("Index");
            }
            return View(EmployeeVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id , "Edit");  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeVM EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = mapper.Map<EmployeeVM, Employee>(EmployeeVM);
                    await UnitOfWork.EmployeeRepository.Update(employee);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(EmployeeVM);
                }
            }
            return View(EmployeeVM);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var employee =await UnitOfWork.EmployeeRepository.Get(id);
            var EmployeeVM = mapper.Map<Employee, EmployeeVM>(employee);

            if (EmployeeVM == null)
                return NotFound();
            ViewBag.Departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(await UnitOfWork.DepartmentRepository.GetAll());
            return View(EmployeeVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, EmployeeVM EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return NotFound();
            try
            {
                var employee = mapper.Map<EmployeeVM, Employee>(EmployeeVM);
                if (EmployeeVM.ImageName!="" && EmployeeVM.ImageName != null)
                {
                    DocumentSetting.DeleteFile(EmployeeVM.ImageName, "Imgs");
                }
                await UnitOfWork.EmployeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(EmployeeVM);
            }
        }
    }
}
