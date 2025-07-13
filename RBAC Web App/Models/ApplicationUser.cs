using Microsoft.AspNetCore.Identity;

namespace RBACWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // You can add more fields here later
        // public string? ProfileImageUrl { get; set; }
    }
}
