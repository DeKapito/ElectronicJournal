﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ElectronicJournal.Models;
//using ElectronicJournal.ViewModels;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace ElectronicJournal.Controllers
//{
//    public class RolesController : Controller
//    {
//        RoleManager<IdentityRole> _roleManager;
//        UserManager<User> _userManager;

//        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//        }

//        public IActionResult Index()
//        {
//            return View(_roleManager.Roles.ToList());
//        }

//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(string name)
//        {
//            if (!string.IsNullOrEmpty(name))
//            {
//                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
//                if (result.Succeeded)
//                {
//                    return RedirectToAction("Index");
//                }
//                else
//                {
//                    foreach (var error in result.Errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error.Description);
//                    }
//                }
//            }
//            return View(name);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(string id)
//        {
//            IdentityRole role = await _roleManager.FindByIdAsync(id);
//            if (role != null)
//            {
//                IdentityResult result = await _roleManager.DeleteAsync(role);
//            }
//            return RedirectToAction("Index");
//        }

//        [HttpGet]
//        public IActionResult UserList()
//        {
//            return View(_userManager.Users.ToList());
//        }

//        [HttpGet]
//        public async Task<IActionResult> Edit(string userId)
//        {
//            User user = await _userManager.FindByIdAsync(userId);
//            if (user != null)
//            {
//                var userRoles = await _userManager.GetRolesAsync(user);
//                var allRoles = _roleManager.Roles.ToList();
//                ChangeRoleViewModel model = new ChangeRoleViewModel
//                {
//                    UserId = user.Id,
//                    UserEmail = user.Email,
//                    UserRoles = userRoles,
//                    AllRoles = allRoles
//                };

//                return View(model);
//            }

//            return NotFound();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(string userId, List<string> roles)
//        {
//            User user = await _userManager.FindByIdAsync(userId);

//            if (user != null)
//            {
//                var userRoles = await _userManager.GetRolesAsync(user);
//                var allRoles = _roleManager.Roles.ToList();
//                //Список ролей, які додали
//                var addedRoles = roles.Except(userRoles);
//                //Ролі, які видалили
//                var removedRoles = userRoles.Except(roles);

//                await _userManager.AddToRolesAsync(user, addedRoles);
//                await _userManager.RemoveFromRolesAsync(user, removedRoles);

//                return RedirectToAction("UserList");
//            }

//            return NotFound();
//        }
//    }
//}