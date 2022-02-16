using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoshakMonix.Models.Entities;
using PoshakMonix.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UnitOfWork.Repositories;

namespace PoshakMonix.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender _sender;

        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager, IEmailSender sender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._sender = sender;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            #region Validation
            if (!ModelState.IsValid) return View(model);
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "لطفا نام کاربری و ایمیل را وارد کنید");
                return View(model);
            }
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "رمز عبور و تکرار رمز عبور را وارد کنید");
                return View(model);
            }
            #endregion


            
            var registeredUser = await userManager.FindByEmailAsync(model.Email) ??
                await userManager.FindByNameAsync(model.UserName);

            if (registeredUser != null)
            {
                ViewBag.Message = "حساب کاربری با این نام کاربری یا ایمیل از قبل موجود است";
                return View(model);
            }

            #region Register
            //Login User
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, model.Password);

            // Show Errors(if errors exist)
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ModelState.AddModelError("Password" , "توجه کنید : رمز عبور باید حداقل 8 حرف و دارای حرف و عدد باشه");
                return View(model);
            }

            var roleResult = await userManager.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            #endregion


            //Everything is Ok :)       Go To Success Page
            return RedirectToAction("SuccessRegister");
        }




        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var model = new LoginViewModel()
            {
                ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).First()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            model.ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).First();

            var user = await userManager.FindByEmailAsync(model.UserNameOrEmail) ?? 
                await userManager.FindByNameAsync(model.UserNameOrEmail);

            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                return Redirect("/");
            }

            ModelState.AddModelError("", "مشکلی وجود دارد");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action(nameof(ResetPassword), "Account",
                    new { email = user.Email, token = token }, Request.Scheme);

                string emailText
                    = $" سلام کاربر گرامی، برای تغییر رمز عبور خود به <a href=\" {link} \"> این لینک</a> مراجعه کنید";


                await _sender.SendEmailAsync(user.Email, "فراموشی رمز عبور", emailText, true);

                ViewBag.Message = "ایمیل تغییر رمز برای شما ارسال شد";
            }
            else
            {
                ViewBag.Message = "عملیات با شکست مواجه شد، لطفا از درستی ایمیل خود اطمینان حاصل کنید";
            } 

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "تغییر رمز با موفقیت انجام شد";
                        return RedirectToAction(nameof(Login));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider)
        {
            var returnUrl = Url.Action("ExternalLoginCallback", "Account");

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");


            if (remoteError != null)
            {
                ViewBag.Message = $"مشکلی به وجود آمد: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ViewBag.Message = $"مشکلی به وجود آمد";
                return RedirectToAction(nameof(Login));
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
            if (!signInResult.Succeeded)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User()
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);

                        await userManager.AddToRoleAsync(user, "User");
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, true);
                }
            }

            return LocalRedirect(returnUrl);
        }
    }
}
