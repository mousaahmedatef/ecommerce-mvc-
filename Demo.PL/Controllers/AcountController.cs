using Demo.BLL.Helper;
using Demo.BLL.Models;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AcountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AcountController(UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> signInManager )
        {
            this.userManger = userManger;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree
                };

                var result = await userManger.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
           
            if (ModelState.IsValid)
            {
                var user = await userManger.FindByEmailAsync(model.Email);
                if (user!= null)
                {
                    var password = await userManger.CheckPasswordAsync(user, model.Password);
                    if (password)
                    {
                        //false-- falseمن اول مره الايميل  بتاعو هيتعملو لوك ف عشان كدا عملتها ب signIn رابع قيمه دى لو عملتها ب ترو اليوزر لو مقدرش يعمل 
                        var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                        
                    }

                }   
                
            }

            ViewData["ErrorMessage"] = "Email Or Password Is Invalid";
            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();

        }

        [HttpPost]
        public async  Task<IActionResult> ForgetPassword(ForgetPasswordVM model) 
        {
            if (ModelState.IsValid)
            {
                var user = await userManger.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManger.GeneratePasswordResetTokenAsync(user); //Token is valid to this user

                    //Url.Action(Action , Controller , parameter to this action , header of link-->like http or https)
                    //Request.Scheme -- https هيبقا  //https://localhost:44354/Acount/ForgetPassword الى هما مثلا ForgetPassword الى هوا  request الي جايلي ف الheader هنا انا بقولو خد ال
                    var resetPasswordLink = Url.Action("ResetPassword", "Acount", new { Email = model.Email , Token=token},Request.Scheme);
                    //resetPasswordLink --الى انا هبعتهوله ف الميل عشان يدوس عليه ويروح يعمل ريست للباسورد url دا ال 
                    //https://localhost:44354/Acount/ResetPassword?Email=ahmed@gmail.com&token=sefsf098ffs
                    //token -- دى انا باعتها مع الريكوست ليه؟
                    // url يغير الميل الى ف ال url لان اليوزر مثلا ممكن بعد م يدوس على اللينك وبروح على ال 
                    //ويدوس انتر فكدا هيعدل الباسوورد بتاع ميل تاني خالص hagar@gmail.com يعدلو وخليه ahmed@gmail.com بدل مثلا م يخليه 
                    //مربوطه بالميل الى ف اللينك فلو غير ف الميل الى ف اللينك  token ف انا وانا ببعت اللينك الى هيدوس عليه ببعت معاه 
                    //الى موجوده ف اللينك مش بتاعه الميل الى موجود ف اللينك token هيطلعلو ايررو يقولو ال 
                    var email = new Email()
                    {
                        Title = "Reset Password",
                        To = model.Email,
                        Body = resetPasswordLink
                    };

                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CompleteForgetPassword)); 
                }

                ModelState.AddModelError(string.Empty, "Email is not Existed!");

            }
            return View(model);
        }

        public IActionResult CompleteForgetPassword() 
        {
            return View();
        }

        public IActionResult ResetPassword(string email , string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManger.FindByEmailAsync(model.Email);
                if (user != null) 
                {
                    var result = await userManger.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ResetPasswordDone));
                    }

                    foreach (var Error in result.Errors)
                        ModelState.AddModelError(string.Empty, Error.Description);
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "This Email is not existed");
            }
            return View(model);
        }

        public IActionResult ResetPasswordDone()
        {
            return View();
        }
    }
}
