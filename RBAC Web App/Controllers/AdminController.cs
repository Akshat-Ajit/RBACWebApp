using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RBACWebApp.Models;
using RBACWebApp.Models.ViewModels;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    [Authorize(Roles = "Admin")]
    public IActionResult Dashboard()
    {
        return View();
    }

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> ManageUsers()
    {
        var users = _userManager.Users.ToList();
        var userRolesViewModel = new List<UserWithRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRolesViewModel.Add(new UserWithRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = roles
            });
        }

        return View(userRolesViewModel);

    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || string.IsNullOrWhiteSpace(newRole))
        {
            return RedirectToAction("ManageUsers");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
            ModelState.AddModelError("", "Failed to remove existing roles.");
            return RedirectToAction("ManageUsers");
        }

        var addResult = await _userManager.AddToRoleAsync(user, newRole);

        if (!addResult.Succeeded)
        {
            ModelState.AddModelError("", "Failed to assign new role.");
        }

        return RedirectToAction("ManageUsers");
    }

}
