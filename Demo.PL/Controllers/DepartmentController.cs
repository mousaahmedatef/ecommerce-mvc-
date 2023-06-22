using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Models;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper )
        {
            this.UnitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(await UnitOfWork.DepartmentRepository.GetAll());
                return View(departments);

            }
            else
            {
                var departments = mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(await UnitOfWork.DepartmentRepository.SearchByName(SearchValue));
                return View(departments);
            }
            
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var department = await UnitOfWork.DepartmentRepository.Get(id);
            var DepartmentVM = mapper.Map<Department, DepartmentVM>(department);
            if (DepartmentVM == null)
                return NotFound();
            return View(DepartmentVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentVM departmentVM)
        {
            Console.WriteLine(departmentVM);

            if (ModelState.IsValid) // Server Side Validation
            {
                var department = mapper.Map<DepartmentVM, Department>(departmentVM);
                await UnitOfWork.DepartmentRepository.Add(department);
                return RedirectToAction("Index");
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var department = await UnitOfWork.DepartmentRepository.Get(id);
            var DepartmentVM  = mapper.Map<Department,DepartmentVM>(department);

            if (DepartmentVM == null)
                return NotFound();
            return View(DepartmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //request الى فوقى دى انا بكتبها عشان امنع اى حد انو يعمل 
        //https://localhost:44358/DepartmentVM/Edit/1 مثلا كدا postman من برا الابلكيشن زي انى اعمل ريكوست من ال 
        //وهيخش عادى عل الاكشن دى ...ف انا بقولو لا انا عايز الريكوست يجيليلى من الفورم الى ف الفيو الى عندى فقط ومينفعش ييجي من حته تانيه
        //bad request دا هيديني url فلو جيت عملت ريكوست ف البوست مان بال 
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentVM departmentVM)
        {
            if (id != departmentVM.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var department = mapper.Map<DepartmentVM, Department>(departmentVM);

                    await UnitOfWork.DepartmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(departmentVM);
                }
            }
            return View(departmentVM);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var department =await UnitOfWork.DepartmentRepository.Get(id);
            var DepartmentVM = mapper.Map<Department, DepartmentVM>(department);

            if (DepartmentVM == null)
                return NotFound();
            return View(DepartmentVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        //[FromRoute] url ألى ف ال segment دى بستخدمها لو عايز اجيب داتا موجوده ف ال
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int? id, DepartmentVM departmentVM)
        {
            if (id != departmentVM.Id)
                return NotFound();
            try
            {
                var department = mapper.Map<DepartmentVM, Department>(departmentVM);
                await UnitOfWork.DepartmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(departmentVM);
            }
        }
    }
}
