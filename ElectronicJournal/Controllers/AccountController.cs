using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Models;
using ElectronicJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailNotification()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email,
                                        Name = model.Name, LastName = model.LastName,
                                        Group = model.Group};

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Student");    /////////
                result = await _userManager.UpdateAsync(user);   /////////////

                if (result.Succeeded)
                {
                    var codeForConfirm = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                    new { userId = user.Id, code = codeForConfirm }, //////////////////////
                                                    protocol: HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Підтвердіть свій E-mail",
                                                        "Для підтвердження реєстрації перейдіть за посиланням: <a href='" + callbackUrl + "'>link</a>");

                    return RedirectToAction("EmailNotification");
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // чи підтверджений E-mail
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Ви не підтвердили свій E-mail");

                        var codeForConfirm = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                                        new { userId = user.Id, code = codeForConfirm }, //////////////////////
                                                        protocol: HttpContext.Request.Scheme);

                        EmailService emailService = new EmailService();
                        await emailService.SendEmailAsync(model.Email, "Підтвердіть свій E-mail",
                                                            "Для підтвердження E-mail перейдіть за посиланням: <a href='" + callbackUrl + "'>link</a>");

                        return RedirectToAction("EmailNotification");
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // чи належить URL додатку
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін або пароль");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            if(!user.EmailConfirmed)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        var codeForConfirm = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = codeForConfirm }, protocol: HttpContext.Request.Scheme);

        //        EmailService emailService = new EmailService();
        //        await emailService.SendEmailAsync(model.Email, "Підтвердіть свій аккаунт",
        //                                            "Для скидання паролю перейдіть за посиланням: <a href='" + callbackUrl + "'>link</a>. "
        //                                            + "На ваш E-mail прийде посилання для скидання паролю.");

        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string code = null)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(string userId, string code)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByNameAsync(model.Email);

        //    if (user == null)
        //    {
        //        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}
    }
}