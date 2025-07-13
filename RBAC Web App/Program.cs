using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RBAC_Web_App.Data;
using RBACWebApp.Data;
using RBACWebApp.Models;
using RBACWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------
// 1. Add MySQL DB Context
// ------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));
builder.Services.AddTransient<IEmailSender, EmailSender>();

// ------------------------------------
// 2. Add Identity with Roles
// ------------------------------------
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ------------------------------------
// 3. Add MVC
// ------------------------------------
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ------------------------------------
// 4. Seed Roles
// ------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAsync(services);
}

// ------------------------------------
// 5. Middleware
// ------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
