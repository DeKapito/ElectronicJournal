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
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ElectronicJournalContext _context;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ElectronicJournalContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ViewBag.groups = _context.Group.ToList();
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.groups = _context.Group.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    LastName = model.LastName,
                    GroupID = model.GroupID
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "Student");
                result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            ViewBag.groups = _context.Group.ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();

            EditUserViewModel model = new EditUserViewModel {
                                                Id = user.Id,
                                                Name = user.Name,
                                                LastName = user.LastName,
                                                GroupID = user.GroupID,
                                                Email = user.Email,
                                                UserRoles = userRoles,
                                                AllRoles = allRoles};

            ViewBag.groups = _context.Group.ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model, List<string> roles)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.LastName = model.LastName;
                    user.GroupID = model.GroupID;
                    user.Email = model.Email;

                    var userRoles = await _userManager.GetRolesAsync(user);
                    var allRoles = _roleManager.Roles.ToList();

                    //Список ролей, які додали
                    var addedRoles = roles.Except(userRoles);
                    //Ролі, які видалили
                    var removedRoles = userRoles.Except(roles);

                    await _userManager.AddToRolesAsync(user, addedRoles);
                    await _userManager.RemoveFromRolesAsync(user, removedRoles);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}