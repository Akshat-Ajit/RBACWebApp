namespace RBACWebApp.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
