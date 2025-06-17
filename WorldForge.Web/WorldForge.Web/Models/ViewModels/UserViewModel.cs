namespace WorldForge.Web.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }               // IdentityUser.Id
        public string UserName { get; set; }         // IdentityUser.UserName
        public string Email { get; set; }            // IdentityUser.Email
        public bool EmailConfirmed { get; set; }     // IdentityUser.EmailConfirmed
        public bool IsLockedOut { get; set; }        // Lockout status
        public List<string> Roles { get; set; } = new(); // Current roles assigned

        // Optional: if you want to support role editing in the UI
        public List<string> AllRoles { get; set; } = new(); // All available roles
        public List<string> SelectedRoles { get; set; } = new(); // For editing roles
    }
}
