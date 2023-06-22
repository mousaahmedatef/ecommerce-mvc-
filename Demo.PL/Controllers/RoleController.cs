using Demo.BLL.Models;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    // [Authorize(Roles="Admin , Hr")] دول مش واحد بس roles هنا بقولو ان لازم يكون عندو الاتنين

    //دى او دى ف بعملها كدا role طيب لو انا عايز اقولو لو عندو ال
    //[Authorize(Roles="Admin)] 
    //[Authorize(Roles="Hr")]

    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                return View(roleManager.Roles);
            }
            else
            {
                var role = await roleManager.FindByNameAsync(SearchValue);
                return View(new List<IdentityRole>() { role });
            }
           
          
        }


        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                
            }
            return View(role);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var user = await roleManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(ViewName, user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, IdentityRole UpdatedRole)
        {
            if (id != UpdatedRole.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {

                    var role = await roleManager.FindByIdAsync(id);
                    role.Name = UpdatedRole.Name;
                    role.NormalizedName = UpdatedRole.Name.ToUpper();

                    var result = await roleManager.UpdateAsync(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return View(UpdatedRole);

        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] string id, IdentityRole deletedRole)
        {
            if (id != deletedRole.Id)
                return NotFound();
            try
            {
                IdentityResult result = await roleManager.DeleteAsync(deletedRole);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(deletedRole);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            if (RoleId == null)
                return NotFound();
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return NotFound();
            List<UserInRoleVM> Users = new List<UserInRoleVM>();
            foreach (var User in userManager.Users)
            {

                var userInRole = new UserInRoleVM()
                {
                    UserId = User.Id,
                    UserName = User.UserName
                };
                if (await userManager.IsInRoleAsync(User, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;
                Users.Add(userInRole);
            }
            return View(Users);

            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleVM> model, string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            if (role == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    var user = await userManager.FindByIdAsync(item.UserId);
                    if (item.IsSelected == true && !(await userManager.IsInRoleAsync(user, role.Name)))
                        await userManager.AddToRoleAsync(user, role.Name);
                    if (item.IsSelected == false && (await userManager.IsInRoleAsync(user, role.Name)))
                        await userManager.RemoveFromRoleAsync(user, role.Name);

                }

                return RedirectToAction("Edit", "Role", new { id = RoleId });

            }
            return View(model); 
        }
    }
}
