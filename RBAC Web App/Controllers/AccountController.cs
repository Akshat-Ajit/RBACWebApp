using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RBACWebApp.Models;
using RBACWebApp.Services;

namespace RBACWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager,
                                 IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // =================== LOGIN ===================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            if (!await _userManager.IsEmailConfirmedAsync(user) && user.Email != "admin@rbac.com")
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email before logging in.");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                    return RedirectToAction("Dashboard", "Admin");

                if (roles.Contains("Manager"))
                    return RedirectToAction("Dashboard", "Manager");

                return RedirectToAction("Dashboard", "User");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // =================== LOGOUT ===================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // =================== REGISTER ===================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullname
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Default to "User" role
                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by clicking this <a href='{confirmationLink}'>link</a>");

                ViewBag.Message = "Registration successful! Please check your email to confirm your account.";
                return View("CheckEmail");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // =================== CONFIRM EMAIL ===================
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View("Error");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
    }
}
