using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Manager")]
public class ManagerController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
}
