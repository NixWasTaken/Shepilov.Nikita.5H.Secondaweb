using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shepilov.Nikita._5H.Secondaweb.Models;
using Microsoft.AspNetCore.Authorization;
using Shepilov.Nikita.dto;

using Microsoft.AspNetCore.Identity;

namespace Shepilov.Nikita._5H.Secondaweb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (ModelState.IsValid)
            {
               var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

               if (result.Succeeded) {
                   // Se l'utente fa login correttamente, entra.
                   return RedirectToAction("Index", "Home");
               }
               else{

                   // ...altrimenti meglio non dare troppo info a chi ci prova
                   // meglio un generico errore,
                   ModelState.AddModelError(string.Empty, "Login error");
               }
            }
            return View(user); 
        }

        public IActionResult Registra()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registra(RegistraDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                     //await _signInManager.SignInAsync(user, isPersistent: false);
                     return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }
    }
}